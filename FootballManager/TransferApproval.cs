using System;

namespace FootballManager
{
    /// <summary>
    /// 转会审批
    /// </summary>
    public class TransferApproval
    {
        private const int RemainingTotalBudget = 300;

        private readonly IPhysicalExmination _physicalExmination;

        private readonly TransferPolicyEvaluator _transferPolicyEvaluator;

        public bool PlayerHealthChecked { get;private set; }

        public TransferApproval(IPhysicalExmination physicalExmination, TransferPolicyEvaluator transferPolicyEvaluator)
        {
            _physicalExmination = physicalExmination ?? throw new ArgumentNullException(nameof(physicalExmination));
            _transferPolicyEvaluator = transferPolicyEvaluator;

            _physicalExmination.HealthChecked += _physicalExmination_HealthChecked;
        }

        private void _physicalExmination_HealthChecked(object sender, EventArgs e)
        {
            PlayerHealthChecked = true;
        }

        public TransferResult Evaluate(TransferApplication transfer)
        {
            if (!_transferPolicyEvaluator.IsInTransferPeriod())
            {
                return TransferResult.Postponed;
            }

            if (!_physicalExmination.MedicalRoom.Status.IsAvailable)
            {
                return TransferResult.Postponed;
            }

            bool isHealthy;
            try
            {
                isHealthy = _physicalExmination.IsHealthy(transfer.PalyerAge, transfer.PlayerStrength, transfer.PlayerSpeed);
            }
            catch (Exception)
            {
                return TransferResult.Postponed;
            }

            //var isHealthy = true;
            //_physicalExmination.IsHealthy(transfer.PalyerAge, transfer.PlayerStrength, transfer.PlayerSpeed, out isHealthy);
            if (!isHealthy)
            {
                _physicalExmination.PhysicalGrade = PhysicalGrade.Failed;
                return TransferResult.Rejected;
            }
            else
            {
                if(transfer.PalyerAge < 25)
                {
                    _physicalExmination.PhysicalGrade = PhysicalGrade.Superb;
                }
                else
                {
                    _physicalExmination.PhysicalGrade = PhysicalGrade.Passed;
                }
            }

            var totalTransferFee = transfer.TranferFee + transfer.ContractYears * transfer.AnnualSalary;

            if(RemainingTotalBudget < totalTransferFee)
            {
                return TransferResult.Rejected;
            }

            if(transfer.PalyerAge < 30)
            {
                return TransferResult.Approved;
            }

            if(transfer.IsSuperStar)
            {
                return TransferResult.ReferredToBoss;
            }

            return TransferResult.Rejected;
        }
    }
}
