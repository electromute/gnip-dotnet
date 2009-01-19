using System;
using NUnit.Framework;
namespace Gnip.Client
{
	[TestFixture]
	public class GnipConnectionFilterTunnelingViaPostTestCase : GnipConnectionFilterTestCase
	{
        [TestFixtureSetUp]
		public override void SetUp()
		{
			base.SetUp();
			config.TunnelOverPost = true;
		}
	}
}