using System;
using System.ComponentModel;

namespace RepairRequestsApp.Models
{
    public class RepairRequest
    {
        [DisplayName("№")]
        public int Id { get; set; }
        
        [DisplayName("Номер помещения")]
        public string RoomNumber { get; set; }
        
        [DisplayName("Описание")]
        public string Description { get; set; }
        
        [DisplayName("Заявитель")]
        public string RequestedBy { get; set; }
        
        [DisplayName("Дата заявки")]
        public DateTime RequestDate { get; set; }
        
        [DisplayName("Статус")]
        public string Status { get; set; }
        
        [DisplayName("Приоритет")]
        public string Priority { get; set; }
        
        [DisplayName("Телефон")]
        public string ContactPhone { get; set; }
        
        [DisplayName("Дата выполнения")]
        public DateTime? CompletionDate { get; set; }
        
        [DisplayName("Примечания")]
        public string Notes { get; set; }
    }
}