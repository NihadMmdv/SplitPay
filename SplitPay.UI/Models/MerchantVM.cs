using SplitPay.DAL.Models;

namespace SplitPay.UI.Models
{
    public class MerchantVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TerminalNo { get; set; }
        public string AppUserId { get; set; }
    }
}
