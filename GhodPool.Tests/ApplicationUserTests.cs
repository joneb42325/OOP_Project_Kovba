using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Project_Kovba.Models;
namespace GhodPool.Tests
{
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void FullName_SetValidName_Success()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            user.FullName = "Іван Петренко";

            // Assert
            Assert.AreEqual("Іван Петренко", user.FullName);
        }

        [TestMethod]
        public void FullName_SetShortName_ThrowsArgumentException()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                user.FullName = "ABFD"
            );
        }

        [TestMethod]
        public void FullName_SetEmpty_ThrowsArgumentException()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
                user.FullName = "   "
            );
        }

        [TestMethod]
        public void ChangeFullName_ValidValue_SetsCorrectly()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act
            user.ChangeFullName("Марія Іваненко");

            // Assert
            Assert.AreEqual("Марія Іваненко", user.FullName);
        }

        [TestMethod]
        public void ChangeFullName_TooShort_ThrowsArgumentException()
        {
            // Arrange
            var user = new ApplicationUser();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => user.ChangeFullName("Alex"));
        }

    }

}
