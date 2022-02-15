using System;

namespace FootballManager
{
    public class TransferPolicyEvaluator
    {
        /// <summary>
        /// 当前是否处于转会期
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool IsInTransferPeriod()
        {
            throw new NotImplementedException();
        }

        protected virtual bool IsBannedFromTransferring()
        {
            throw new NotImplementedException();
        }
    }
}
