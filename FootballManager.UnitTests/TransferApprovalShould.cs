using Moq;
using Moq.Protected;
using System;
using Xunit;

namespace FootballManager.UnitTests
{
    public class TransferApprovalShould
    {
        [Fact]
        public void ApproveYoundCheapPlayerTransfer()
        {
            var mockMedicalRoomStatus = new Mock<IMedicalRoomStatus>();
            mockMedicalRoomStatus.Setup(x => x.IsAvailable).Returns(true);

            var mockMedicalRoom = new Mock<IMedicalRoom>();
            mockMedicalRoom.Setup(x => x.Status).Returns(mockMedicalRoomStatus.Object);

            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.Setup(x => x.MedicalRoom).Returns(mockMedicalRoom.Object);

            // ����������Ա���ϸ�
            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var emreTransfer = new TransferApplication
            {
                PlayerName = "Emre Can",
                PalyerAge = 24,
                TranferFee = 0,
                AnnualSalary = 4.25m,
                ContractYears = 4,
                IsSuperStar = false,
                PlayerStrength = 80,
                PlayerSpeed = 75
            };

            var result = approval.Evaluate(emreTransfer);

            Assert.Equal(TransferResult.Approved, result);
        }

        [Fact]
        public void ReferredToBossWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.Is<int>(age => age < 35), It.IsIn<int>(80, 85, 90), It.IsInRange<int>(75, 99, Moq.Range.Inclusive)))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 80,
                PlayerSpeed = 75
            };

            var result = approval.Evaluate(cr7Transfer);

            Assert.Equal(TransferResult.ReferredToBoss, result);
        }

        /// <summary>
        /// ����ֵ�仯����
        /// </summary>
        [Fact]
        public void PhysicalGradeShouldPassWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 80,
                PlayerSpeed = 75
            };

            var result = approval.Evaluate(cr7Transfer);

            Assert.Equal(PhysicalGrade.Passed, mockPhysicalExmination.Object.PhysicalGrade);
        }

        /// <summary>
        /// ȷ�Ϸ���������
        /// </summary>
        [Fact]
        public void ShouldPhysicalExmineWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            // �ж�������ķ�����ִ��, Ҳ����ʹ��It����в���ƥ��.
            mockPhysicalExmination.Verify(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        /// <summary>
        /// ȷ�����Է��� Get
        /// </summary>
        [Fact]
        public void ShouldCheckMedicalRoomIsAvailableWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            mockPhysicalExmination.VerifyGet(x => x.MedicalRoom.Status.IsAvailable);
        }

        /// <summary>
        /// ȷ�����Է��� Set
        /// </summary>
        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            mockPhysicalExmination.VerifySet(p => p.PhysicalGrade = PhysicalGrade.Passed);
        }

        /// <summary>
        /// �׳��쳣
        /// </summary>
        [Fact]
        public void PostponedWhenTransferringChildPlayer()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.Is<int>(age => age < 16), It.IsAny<int>(), It.IsAny<int>()))
                //.Throws<Exception>();
                .Throws(new Exception("The player is still a child"));

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Some Child Player",
                PalyerAge = 13,
                TranferFee = 0,
                AnnualSalary = 0.1m,
                ContractYears = 3,
                IsSuperStar = false,
                PlayerStrength = 40,
                PlayerSpeed = 50
            };

            var result = approval.Evaluate(cr7Transfer);

            Assert.Equal(TransferResult.Postponed, result);
        }

        /// <summary>
        /// Events
        /// </summary>
        [Fact]
        public void ShouldPlayerHealthCheckedWhenTransferringSuperStar()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);
            //.Raises(x => x.HealthChecked += null, EventArgs.Empty);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            mockPhysicalExmination.Raise(x => x.HealthChecked += null, EventArgs.Empty);

            Assert.True(approval.PlayerHealthChecked);
        }

        /// <summary>
        /// �������õĲ�ͬ����ֵ
        /// </summary>
        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar_Sequence()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .SetupSequence(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true)
                .Returns(false);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result1 = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.ReferredToBoss, result1);

            var result2 = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.Rejected, result2);
        }

        /// <summary>
        /// Mock ��ʵ�ֽӿڵķ���
        /// </summary>
        [Fact]
        public void ShouldPostponedWhenNotInTransferPeriod()
        {
            var mockPhysicalExmination = new Mock<IPhysicalExmination>();

            mockPhysicalExmination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);

            mockPhysicalExmination
                .Setup(x => x.MedicalRoom.Status.IsAvailable).Returns(true);

            mockPhysicalExmination
                .Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            var mockTransferPolicyEvaluator = new Mock<TransferPolicyEvaluator>();

            // mock public, ��Ҫʱvirtual
            mockTransferPolicyEvaluator.Setup(x => x.IsInTransferPeriod()).Returns(false);

            // mock protected, ��Ҫʱvirtual
            mockTransferPolicyEvaluator
                .Protected()
                .Setup<bool>("IsBannedFromTransferring")
                .Returns(true);

            var approval = new TransferApproval(mockPhysicalExmination.Object, mockTransferPolicyEvaluator.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.Postponed, result);
        }

        [Fact]
        public void LinqToMocks()
        {
            var mockPhysicalExmination = Mock.Of<IPhysicalExmination>(
                me => me.MedicalRoom.Status.IsAvailable == true && me.PhysicalGrade == PhysicalGrade.Passed && me.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) == true
                );

            var mockTransferPolicyEvaluator = Mock.Of<TransferPolicyEvaluator>(
                me=> me.IsInTransferPeriod() == true);

            var approval = new TransferApproval(mockPhysicalExmination, mockTransferPolicyEvaluator);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PalyerAge = 33,
                TranferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.ReferredToBoss, result);
        }
    }
}
