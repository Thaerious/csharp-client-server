using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace frar.clientserver.test;

///
/// Create a Router instance and call the methods by processing packets.
/// The RouterTest class invokes the methods on Parent.
/// To test just this class:
/// dotnet test --filter ClassName=frar.clientserver.test.RouterTest
[TestClass]
public class RouterTest {

    [TestMethod]
    public void Set_Int(){
        var router = new RouterImpl();
        router.Process(
            new Packet("setint")
            .Set("value", 3)
        );
        Assert.AreEqual(3, router.i);
    }

    [TestMethod]
    public void Set_Object(){
        var router = new RouterImpl();
        router.Process(
            new Packet("setsimple")
            .Set("value", new SimpleObject(2, 5))
        );
        Assert.AreEqual(2, router?.simple?.a);
    }

    [TestMethod]
    public void Set_Array(){
        var router = new RouterImpl();
        router.Process(
            new Packet("setarray")
            .Set("value", new int[]{3, 5, 7})
        );
        Assert.AreEqual(5, router?.array?[1]);
    }

    [TestMethod]
    public void Set_Int_Anon_Args(){
        var router = new RouterImpl();
        router.Process(new Packet("setint", 3));
        Assert.AreEqual(3, router.i);
    }

    [TestMethod]
    [ExpectedException(typeof(MissingParameterException))]
    public void Set_Int_Anon_Args_Missing(){
        var router = new RouterImpl();
        router.Process(new Packet("setint"));
        Assert.AreEqual(3, router.i);
    }

    [TestMethod]
    public void Set_Int_Anon_Args_Multiple_Invoke(){
        var router = new RouterImpl();
        var packet = new Packet("setint", 3);
        router.Process(packet);
        router.Process(packet);
        Assert.AreEqual(3, router.i);
    }    
}

public class RouterImpl : Router {
    public int i = 0;
    public SimpleObject? simple;
    public int[]? array;

    [Route]
    public void SetArray(int[] value){
        this.array = value;
    }

    [Route]
    public void SetSimple(SimpleObject value){
        this.simple = value;
    }

    [Route]
    public virtual void SetInt(int value){
        this.i = value;
    }
}