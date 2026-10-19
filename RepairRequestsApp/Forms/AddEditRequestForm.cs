using System;
using System.Windows.Forms;
using RepairRequestsApp.Models;
using RepairRequestsApp.Services;

namespace RepairRequestsApp.Forms
{
    public partial class AddEditRequestForm : Form
    {
        private DatabaseService _dbService;
        private int? _requestId;
        private bool _isEditMode;

        public AddEditRequestForm(DatabaseService dbService, int? requestId = null)
        {
            InitializeComponent();
            _dbService = dbService;
            _requestId = requestId;
            _isEditMode = requestId.HasValue;

            if (_isEditMode)
            {
                this.Text = "Редактировать заявку";
                LoadRequestData();
            }
            else
            {
                this.Text = "Добавить заявку";
                dtpRequestDate.Value = DateTime.Now;
                cmbStatus.SelectedIndex = 0;
                cmbPriority.SelectedIndex = 1;
            }
        }

        private void LoadRequestData()
        {
            var request = _dbService.GetRequestById(_requestId.Value);
            if (request != null)
            {
                txtRoomNumber.Text = request.RoomNumber;
                txtDescription.Text = request.Description;
                txtRequestedBy.Text = request.RequestedBy;
                txtContactPhone.Text = request.ContactPhone;
                txtNotes.Text = request.Notes;
                dtpRequestDate.Value = request.RequestDate;
                cmbStatus.SelectedItem = request.Status;
                cmbPriority.SelectedItem = request.Priority;
                
                if (request.CompletionDate.HasValue)
                {
                    chkCompleted.Checked = true;
                    dtpCompletionDate.Value = request.CompletionDate.Value;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            var request = new RepairRequest
            {
                RoomNumber = txtRoomNumber.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                RequestedBy = txtRequestedBy.Text.Trim(),
                ContactPhone = txtContactPhone.Text.Trim(),
                Notes = txtNotes.Text.Trim(),
                RequestDate = dtpRequestDate.Value,
                Status = cmbStatus.SelectedItem.ToString(),
                Priority = cmbPriority.SelectedItem.ToString(),
                CompletionDate = chkCompleted.Checked ? dtpCompletionDate.Value : (DateTime?)null
            };

            if (_isEditMode)
            {
                request.Id = _requestId.Value;
                _dbService.UpdateRequest(request);
            }
            else
            {
                _dbService.AddRequest(request);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("Введите номер помещения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoomNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание проблемы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRequestedBy.Text))
            {
                MessageBox.Show("Введите имя заявителя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRequestedBy.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите статус заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbPriority.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите приоритет заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkCompleted_CheckedChanged(object sender, EventArgs e)
        {
            dtpCompletionDate.Enabled = chkCompleted.Checked;
            if (chkCompleted.Checked && !_isEditMode)
            {
                dtpCompletionDate.Value = DateTime.Now;
            }
        }
    }
}