using Microsoft.VisualStudio.TestTools.UnitTesting;
using utility;
using System;

namespace testproject
{
    [TestClass]
    public class 演示特性在测试中的应用
    {
        [TestMethod]
        public void 使用Person类型()
        {
            var person = new Person()
            {
                Name = "张三",
                BirthDay = new DateTime(1990, 2, 1)
            };
            Assert.AreEqual(new DateTime(1990, 2, 1), person.BirthDay);
        }
    }
}
