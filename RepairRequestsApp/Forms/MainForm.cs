using System;
using System.Windows.Forms;
using RepairRequestsApp.Models;
using RepairRequestsApp.Services;

namespace RepairRequestsApp.Forms
{
    public partial class MainForm : Form
    {
        private DatabaseService _dbService;

        public MainForm()
        {
            _dbService = new DatabaseService();
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _dbService.GetAllRequests();
            dataGridView1.DataSource = requests;
            UpdateStatusBar();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditRequestForm(_dbService);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadRequests();
                MessageBox.Show("Заявка успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRequest = (RepairRequest)dataGridView1.SelectedRows[0].DataBoundItem;
                var editForm = new AddEditRequestForm(_dbService, selectedRequest.Id);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadRequests();
                    MessageBox.Show("Заявка успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту заявку?", "Подтверждение", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    var selectedRequest = (RepairRequest)dataGridView1.SelectedRows[0].DataBoundItem;
                    _dbService.DeleteRequest(selectedRequest.Id);
                    LoadRequests();
                    MessageBox.Show("Заявка успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRequests();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadRequests();
            }
            else
            {
                var searchResults = _dbService.SearchRequests(txtSearch.Text);
                dataGridView1.DataSource = searchResults;
                UpdateStatusBar();
            }
        }

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dbService == null) return;
            
            if (cmbStatusFilter.SelectedIndex == 0)
            {
                LoadRequests();
            }
            else
            {
                var status = cmbStatusFilter.SelectedItem.ToString();
                var filteredRequests = _dbService.FilterByStatus(status);
                dataGridView1.DataSource = filteredRequests;
                UpdateStatusBar();
            }
        }

        private void UpdateStatusBar()
        {
            int totalCount = dataGridView1.Rows.Count;
            toolStripStatusLabel1.Text = $"Всего заявок: {totalCount}";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEdit_Click(sender, e);
            }
        }
    }
}