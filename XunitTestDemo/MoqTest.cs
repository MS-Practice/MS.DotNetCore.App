using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XunitTestDemo
{
    public class MoqTest
    {
        [Fact]
        void TestMoq()
        {
            //提供IFoo类行的模拟实现
            var mock = new Mock<IFoo>();
            //在被模拟的类型上指定一个设置，为了调用一个返回值的方法
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);

            //out 参数
            var outStrings = "ack";
            //TryParse将返回一个true，并且out参数将会返回“ack”,
            mock.Setup(foo => foo.TryParse("ping", out outStrings)).Returns(true);

            // ref 参数
            var instance = new Bar();
            mock.Setup(foo => foo.Submit(ref instance)).Returns(true);

            //访问调用参数当return一个值时
            mock.Setup(x => x.DoSomethingStringy(It.IsAny<String>())).Returns((string s) => s.ToLower());
            //多参重载也可以

            //当调用特殊的参数抛出异常
            mock.Setup(foo => foo.DoSomething("reset")).Throws<InvalidOperationException>();
            mock.Setup(foo => foo.DoSomething("")).Throws(new ArgumentException("command"));

            //延迟评估返回的值
            var count = 1;
            mock.Setup(foo => foo.GetCount()).Returns(() => count);

            //每次调用返回不同的值
            //var mock = new Mock<IFoo>();
            var calls = 0;
            mock.Setup(foo => foo.GetCount())
                .Returns(() => calls)
                .Callback(() => calls++);

            Console.WriteLine(mock.Object.GetCount());

        }
        [Fact]
        public void MatchingArguments()
        {
            var mock = new Mock<IFoo>();
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>())).Returns(true);
            //任何以ref传递的值都会通过(要求moq在版本4.8及以上)
            mock.Setup(foo => foo.Submit(ref It.Ref<Bar>.IsAny)).Returns(true);
            //matching Func<int>, 延迟评估
            mock.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 0))).Returns(true);
            //matching ranges
            mock.Setup(foo => foo.Add(It.IsInRange<int>(0, 10, Range.Inclusive))).Returns(true);
            //matching regex
            mock.Setup(foo => foo.DoSomethingStringy(It.IsRegex("[a-d]+", System.Text.RegularExpressions.RegexOptions.IgnoreCase))).Returns("foo");
        }

        [Fact]
        public void Properties()
        {
            var mock = new Mock<IFoo>();
            mock.Setup(foo => foo.Name).Returns("foo");

            //自动模拟层级
            mock.Setup(foo => foo.Bar.Baz.Name).Returns("baz");

            //期望一个调用将值设置为ifoo
            mock.SetupSet(foo => foo.Name = "foo");
            //或者验证setter
            mock.VerifySet(foo => foo.Name = "foo");
        }
    }
}
