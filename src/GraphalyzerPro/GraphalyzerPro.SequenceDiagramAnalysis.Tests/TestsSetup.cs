using System.Globalization;
using NUnit.Framework;
using ReactiveUI;

namespace GraphalyzerPro.SequenceDiagramAnalysis.Tests
{
    [SetUpFixture]
    public class TestsSetup
    {
        [SetUp]
        public void Setup()
        {
            RxApp.GetFieldNameForPropertyNameFunc = delegate(string name)
                {
                    var nameAsArray = name.ToCharArray();
                    nameAsArray[0] = char.ToLower(nameAsArray[0], CultureInfo.InvariantCulture);
                    return '_' + new string(nameAsArray);
                };
        }
    }
}