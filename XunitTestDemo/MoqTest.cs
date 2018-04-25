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

            //设置属性 让其可以跟踪值（也称为存根）
            mock.SetupProperty(foo => foo.Name);
            //还可以提供默认值
            mock.SetupProperty(foo => foo.Name, "foo");
            //通过上面对属性的设定之后，你可以这么做
            IFoo f = mock.Object;
            //存储初始值
            Assert.Equal("foo", f.Name);

            //初始值改变，新值被赋值
            f.Name = "bar";
            Assert.Equal("bar", f.Name);    //true

            //在模拟中存根所有的属性
            mock.SetupAllProperties();
        }
        [Fact]
        public void Events()
        {

            var mock = new Mock<IFoo>();
            //在mock上触发事件
            mock.Raise(m => m.MyEvent += null, new FooEventArgs());
            //在mock中某个方法发生调用触发事件
            var bar = new Bar();
            mock.Setup(foo => foo.Submit()).Raises(foo => foo.MyEvent += null,EventArgs.Empty);

            //通过传递自定义参数给委托
            mock.Raise(m => m.MyEvent += null, 25, true);
        }
        [Fact]
        public void Callbacks()
        {
            var mock = new Mock<IFoo>();
            var calls = 0;
            var callArgs = new List<string>();

            mock.Setup(foo => foo.DoSomething("ping"))
                .Returns(true)
                .Callback(() => calls++);
            //访问调用参数
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(true)
                .Callback((string s) => callArgs.Add(s));
            //等价泛型方法语法
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>()))
                .Returns(true)
                .Callback<string>(s => callArgs.Add(s));

            //在调用方法之之前指定回调函数
            mock.Setup(foo => foo.DoSomething("ping"))
                .Callback(() => Console.WriteLine("Before returns"))
                .Returns(true)
                .Callback(() => Console.WriteLine("After returns"));
            //回调的方法带有ref/out也是可以的，但是要在4.8版本及以上
            mock.Setup(foo => foo.Submit(ref It.Ref<Bar>.IsAny))
                .Callback(new SubmitCallback((ref Bar bar) => Console.WriteLine("Submiting a Bar!")));
        }
        delegate void SubmitCallback(ref Bar bar);

        [Fact]
        public void Verification()
        {
            var mock = new Mock<IFoo>();
            mock.Verify(foo => foo.DoSomething("ping"));
            mock.Verify(foo => foo.DoSomething("ping"), "error message");
            //验证方法永远不会被调用
            mock.Verify(foo => foo.DoSomething("ping"), Times.Never());
            //至少调一次
            mock.Verify(foo => foo.DoSomething("ping"), Times.AtLeastOnce);
            //验证getter调用
            mock.VerifyGet(foo => foo.Name);
            mock.VerifySet(foo => foo.Name = "foo");
            //用参数匹配验证setter
            mock.VerifySet(foo => foo.Value = It.IsInRange(1, 5, Range.Inclusive));
            //确认除了已经验证过的其他调用之外没有其他调用了（version >= 4.8）
            mock.VerifyNoOtherCalls();
        }
        [Fact]
        public void CustomeMockBehavior()
        {
            //模拟行为就像是真的模拟一样，在没有响应期望值触发异常：在moq中使用“严格”模拟；默认行为是“Loose(松散)”模拟
            //如果没有为成员设置期望，它不会抛出异常并且返回默认值，空数组，枚举等
            var mock = new Mock<IFoo>(MockBehavior.Strict);//模拟默认行为
            //如果没有期望覆盖成员时调用基类实现（又名部分模拟）：默认是false。如果在system.web模拟web/html控件，那这是必须的
            mock = new Mock<IFoo>
            {
                CallBase = true
            };
            //自动递归模拟：一个模拟将会返回一个新的mock为每一个成员，它不需要期望，并且返回的值能继续被mock(注意，不能是值类型)
            mock = new Mock<IFoo>
            {
                DefaultValue = DefaultValue.Mock
            };
            //默认是DefaultValue.Empty
            //这个属性访问将会返回一个新的mock bar,他是可模拟的(mock-able)
            Bar value = mock.Object.Bar;

            //返回的mock被重用，所以进一步访问属性返回相同的mock实例，可以根据需要对这个mock实例可以进行进一步的期望
            var barMock = Mock.Get(value);
            barMock.Setup(b => b.Submit()).Returns(true);
        }
    }
}
