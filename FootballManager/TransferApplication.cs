namespace FootballManager
{
    /// <summary>
    /// 球员转会申请
    /// </summary>
    public class TransferApplication
    {
        public int Id { get; set; }

        public string PlayerName { get; set; }

        public int PalyerAge { get; set; }

        public decimal TranferFee { get; set; }

        public decimal AnnualSalary { get; set; }

        public int ContractYears { get; set; }

        public bool IsSuperStar { get; set; }

        public int PlayerStrength { get; set; }

        public int PlayerSpeed { get; set; }
    }
}
