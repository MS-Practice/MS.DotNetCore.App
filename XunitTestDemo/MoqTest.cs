using Moq;
using Moq.Protected;
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
            mock.Setup(foo => foo.Submit()).Raises(foo => foo.MyEvent += null, EventArgs.Empty);

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

            //返回的mock被重用，所以进一步访问属性返回相同的mock实例，可以根据需要对这个mock实例可以进行设置进一步的期望
            var barMock = Mock.Get(value);
            barMock.Setup(b => b.Submit()).Returns(true);

            //集中模拟实例的创建和管理：你可以在单个地方通过使用MockRepository创建和验证所有的模拟，它允许设置一个模拟行为(MockBehaior),CallBase和DefaultValue是一致的
            var repository = new MockRepository(MockBehavior.Strict)
            {
                DefaultValue = DefaultValue.Mock
            };
            barMock = repository.Create<Bar>(MockBehavior.Loose);
            repository.Verify();
        }

        [Fact]
        public void SomethingElse()
        {
            //设置一个成员返回不同的值/顺序调用抛出异常
            var mock = new Mock<IFoo>();
            mock.SetupSequence(foo => foo.GetCount())
                .Returns(3)
                .Returns(2)
                .Returns(1)
                .Returns(0)
                .Throws(new InvalidOperationException());   //将会在第五次调用的时候抛出异常

            //对受包含的成员设置期望（这里你不能智能感知，所以得用字符串的形式去访问它们）
            var commonMock = new Mock<CommandBase>();
            mock.Protected()
                .Setup<int>("Execute")
                .Returns(5);
            //如果你要参数匹配，那么你必须要用ItExpr而不是It
            //这点在下一个版本计划优化提高(下面是Moq 4.8另一个选择)
            mock.Protected()
                .Setup<bool>("Execute", ItExpr.IsAny<string>())
                .Returns(true);

            mock.Protected().As<CommandBaseProtectedMembers>()
                .Setup(m => m.Execute(It.IsAny<string>()))  //将会设置CommandBase.Execute
                .Returns(true);
        }

        //Moq4.8之后，你可以通过一个完全不相关的类型来设置受保护的成员，该类型有相同的成员，从而提供了智能感知所需的类型信息。你也可以使用接口去设置受保护的通用方法和那些带ref的参数
        interface CommandBaseProtectedMembers
        {
            bool Execute(string arg);
        }
        [Fact]
        public void AdvancedFeture()
        {
            //从已经模拟的实例中 获得这个实例
            IFoo foo = new Mock<IFoo>().Object;
            var fooMock = Mock.Get(foo);
            //实现多接口模拟
            var mock = new Mock<IFoo>();
            var disposableFoo = mock.As<IDisposable>();
            //现在IFoo的模拟也能模拟IDisposable
            disposableFoo.Setup(disposable => disposable.Dispose());
            //实现单模拟多接口
            var mock1 = new Mock<IFoo>();
            mock.Setup(ifoo => ifoo.Name).Returns("Red");
            mock.As<IDisposable>().Setup(disposable => disposable.Dispose());
            //自定义匹配
            mock.Setup(foo2 => foo2.DoSomething(IsLarge())).Throws<ArgumentException>();

            //模拟内部类(Internal):添加下面的自定义特性(指定在AssemblyInfo.cs)到项目包含的内部类——那一个类依赖你的项目是否被强命名
            // This assembly is the default dynamic assembly generated by Castle DynamicProxy, 
            // used by Moq. If your assembly is strong-named, paste the following in a single line:
            //[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]

            //// Or, if your own assembly is not strong-named, omit the public key:
            //[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

            //在Moq4.8以后，你可以创建自己的默认值生成策略（除了 DefaultValue.Empty 和 DefaultValue.Mock）通过子类 DefaultValueProvider，或者是你想要更多的约束，LookupOrFallbackDefaultValueProvider
            var mock2 = new Mock<IFoo>
            {
                DefaultValueProvider = new MyEmptyDefaultValueProvider()
            };
            var name = mock.Object.Name;
            //注意：当你通过模拟消费市，你必须传递mock.Object 而不是 mock自己

        }
        [Fact]
        public void MockLinq()
        {
            //Moq支持Linq查询，通过声明性规范查询
            var queries = Mock.Of<IServiceProvider>(sp => sp.GetService(typeof(IRepository)) == Mock.Of<IRepository>(r => r.IsAuthenticated == true && sp.GetService(typeof(IAuthentication)) == Mock.Of<IAuthentication>(a => a.AuthenticationType == "OAuth")));
            //单个模拟，递归模拟的多个设置
            //ControllerContext
        }

        private string IsLarge()
        {
            return Match.Create<string>(s => !String.IsNullOrEmpty(s) && s.Length > 100);
        }

        class MyEmptyDefaultValueProvider : LookupOrFallbackDefaultValueProvider {
            public MyEmptyDefaultValueProvider()
            {
                base.Register(typeof(string), (type, mock) => "?");
                base.Register(typeof(List<>), (type, mock) => Activator.CreateInstance(type));
            }
        }
    }
}
