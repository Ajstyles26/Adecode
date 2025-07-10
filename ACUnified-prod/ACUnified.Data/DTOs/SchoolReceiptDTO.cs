namespace ACUnified.Data.DTOs;

public class SchoolReceiptDTO
{
    public string SchoolName { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public string ReceiptFor { get; set; }
    public string StudentName { get; set; }
    
    public string Semester { get; set; }
    
    public string Session { get; set; }
    
    public string Matric { get; set; }
    public string Department { get; set; }
    public string Faculty { get; set; }
    public string ReferenceId { get; set; }

    public string Level { get; set; }
    public List<ReceiptItemDTO> Items { get; set; }
    public decimal TotalAmount => Items.Sum(item => item.Amount);
    public string QRCodeData { get; set; }
}