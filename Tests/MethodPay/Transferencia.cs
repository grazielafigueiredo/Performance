using NUnit.Framework;
using DotnetQaFront.Common;
using DotnetQaFront.Pages;
using DotnetQaFront.Models;
using DotnetQaFront.Lib;
using System.Collections.Generic;

namespace DotnetQaFront.Tests.MethodPay
{
    public class Transferencia : BaseTest
    {
        private LoginPage _login;
        private ProductPage _product;
        private PaymentCartPage _cart;
        private TransferenciaPage _transferencia;
        private DataFirstAccess _dataAccess;


        [SetUp]
        public void Before()
        {
            _login = new LoginPage(Browser);
            _product = new ProductPage(Browser);
            _cart = new PaymentCartPage(Browser);
            _transferencia = new TransferenciaPage(Browser);
            _dataAccess = new DataFirstAccess(Browser);
            Database.ClearDataFirstAccess();
            _login.With("user22@gmail.com", "1234");
            Database.RemoveAccount();
        }

        [Test]
        [Category("pagamento")]
        [Category("transferencia")]

        public void PaymnentTransferenciaBradesco()
        {
            var TransferenciaBradescoData = new TransferenciaModel()
            {
                Titular = "Sou titular",
                Agencia = "9876",
                Conta = "1120",
                Digito = "1",
            };

            _product.AddProductCartPay();
            _cart.ConfirmPaymentCart();
            _transferencia.SelectTransferencia();
            _transferencia.TransferenciaBradescoData(TransferenciaBradescoData);
            _dataAccess.InputDataFirstAccess();
            _transferencia.ConfirmPaymentTransferencia();

            Assert.AreEqual("Pedido Recebido!", _transferencia.MessagePaymentSuccess());
        }

        [Category("pagamento")]
        [Category("transferencia")]
        [Test]
        public void PaymnentTransferenciaItau()
        {
            var TransferenciaItauData = new TransferenciaModel()
            {
                Titular = "Meu titulo",
                Agencia = "42424",
                Conta = "11201",
                Digito = "1",
            };

            _product.AddProductCartPay();
            _cart.ConfirmPaymentCart();
            _transferencia.SelectTransferencia();
            _transferencia.TransferenciaItauData(TransferenciaItauData);
            _dataAccess.InputDataFirstAccess();
            _transferencia.ConfirmPaymentTransferencia();

            Assert.AreEqual("Pedido Recebido!", _transferencia.MessagePaymentSuccess());
        }

        [Test]
        [Category("pagamento")]
        [Category("transferencia")]
        public void PaymnentTransferenciaSantander()
        {
            var TransferenciaSantanderData = new TransferenciaModel()
            {
                Cpf = "00000009652",
            };

            _product.AddProductCartPay();
            _cart.ConfirmPaymentCart();
            _transferencia.SelectTransferencia();
            _transferencia.TransferenciaSantanderData(TransferenciaSantanderData);
            _dataAccess.InputDataFirstAccess();
            _transferencia.ConfirmPaymentTransferencia();

            Assert.AreEqual("Pedido Recebido!", _transferencia.MessagePaymentSuccess());
        }

        [Test]
        [Category("pagamento")]
        [Category("transferencia")]
        public void PaymnentTransferenciaBrasil()
        {
            var TransferenciaBrasilData = new TransferenciaModel()
            {
                Titular = "Meu Cartao",
                Agencia = "4242",
                DigitoAgencia = "2",
                Conta = "1120",
                Digito = "1",
            };

            _product.AddProductCartPay();
            _cart.ConfirmPaymentCart();
            _transferencia.SelectTransferencia();
            _transferencia.TransferenciaBrasilData(TransferenciaBrasilData);
            _dataAccess.InputDataFirstAccess();
            _transferencia.ConfirmPaymentTransferencia();

            Assert.AreEqual("Pedido Recebido!", _transferencia.MessagePaymentSuccess());
        }

        [Test]
        [Category("pagamento")]
        [Category("transferencia")]
        public void PaymnentAccountSaved()
        {
            Database.InsertAccount();
            Database.InsertDataFirstAccess();

            List<string> selectBank = new List<string>();
            selectBank.Add("1"); //BB
            selectBank.Add("2"); //Santander
            selectBank.Add("3"); //Itaú
            selectBank.Add("4"); //Bradesco

            foreach (string optionBank in selectBank)
            {
                Browser.Visit("/");
                _product.AddProductCartPay();
                _cart.ConfirmPaymentCart();
                _transferencia.SelectTransferencia();
                _transferencia.SelectBankSaved(optionBank);
                _transferencia.ConfirmPaymentTransferencia();

                Assert.AreEqual("Pedido Recebido!", _transferencia.MessagePaymentSuccess());
            }
        }
        
        [TearDown]
        public void RemoveAccount()
        {
            Database.RemoveAccount();
        }
    }
}