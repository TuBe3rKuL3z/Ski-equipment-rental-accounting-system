using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ski_equipment_rental_accounting_system;

namespace SkiRentUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        //Проверка полное ли имя клиента
        [TestMethod] 
        public void Client_FullName_ShouldCombineNamesCorrectly()
        {
            var client = new Client
            {
                LastName = "Кубанов",
                FirstName = "Альберт",
                SecondName = "Азрет-Алиевич"
            };
            


            Assert.AreEqual("Кубанов Альберт Азрет-Алиевич", client.FullName);
        }

        //Проверка документов

        [TestMethod]
        public void Client_DocumentInfo_ShouldCombineTypeAndNumber()
        {
            var client = new Client
            {
                DocumentType = DocumentTypes.Passport,
                DocumentNumber = "1234567890"
            };

            var documentInfo = client.DocumentInfo;

            Assert.AreEqual("Паспорт: 1234567890", documentInfo);
        }

        //Проверка выдаст ли ошибку при отстутствии фамилии
        [TestMethod]
        public void Client_Validate_ShouldReturnInvalid_ForMissingLastName()
        {
            var client = new Client
            {
                LastName = "",
                FirstName = "Иван",
                SecondName = "Иванович",
                DocumentType = DocumentTypes.Passport,
                DocumentNumber = "1234567890"
            };

            var result = client.Validate();

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Фамилия обязательна для заполнения", result.ErrorMessage);
        }


        //Проверка выдаст ли ошибку при слишком коротком номере документа выдаст ли ошибку
        [TestMethod]
        public void Client_Validate_ShouldReturnInvalid_ForShortDocumentNumber()
        {
            var client = new Client
            {
                LastName = "Иванов",
                FirstName = "Иван",
                SecondName = "Иванович",
                DocumentType = DocumentTypes.Passport,
                DocumentNumber = "12"
            };

            var result = client.Validate();

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Номер документа слишком короткий", result.ErrorMessage);
        }
    }
}
