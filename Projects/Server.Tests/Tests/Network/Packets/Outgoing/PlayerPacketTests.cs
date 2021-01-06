using System;
using Server.HuePickers;
using Server.Network;
using Xunit;

namespace Server.Tests.Network
{
    public class PlayerPacketTests : IClassFixture<ServerFixture>
    {
        [Theory]
        [InlineData(StatLockType.Down, StatLockType.Up, StatLockType.Locked)]
        public void TestStatLockInfo(StatLockType str, StatLockType intel, StatLockType dex)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();
            m.StrLock = str;
            m.IntLock = intel;
            m.DexLock = dex;

            var expected = new StatLockInfo(m).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendStatLockInfo(m);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(200)]
        public void TestChangeUpdateRange(int range)
        {
            var expected = new ChangeUpdateRange(range).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendChangeUpdateRange((byte)range);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TestDeathStatus(bool dead)
        {
            var expected = new DeathStatus(dead).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendDeathStatus(dead);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        [InlineData(100, true)]
        [InlineData(1000, false)]
        public void TestSpecialAbility(int abilityId, bool active)
        {
            var expected = new ToggleSpecialAbility(abilityId, active).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendToggleSpecialAbility(abilityId, active);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0x1000u, "This is a header", "This is a body", "This is a footer")]
        [InlineData(0x1000u, null, null, null)]
        public void TestDisplayProfile(uint serial, string header, string body, string footer)
        {
            Serial m = serial;
            var expected = new DisplayProfile(m, header, body, footer).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendDisplayProfile(m, header, body, footer);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(LRReason.CannotLift)]
        [InlineData(LRReason.TryToSteal)]
        public void TestLiftRej(LRReason reason)
        {
            var expected = new LiftRej(reason).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendLiftReject(reason);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestLogoutAck()
        {
            var expected = new LogoutAck().Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendLogoutAck();

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(4, 5, 6)]
        [InlineData(0x1234, 0x5678, 0x9ABC)]
        public void TestWeather(int type, int density, int temp)
        {
            var expected = new Weather(type, density, temp).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendWeather((byte)type, (byte)density, (byte)temp);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(100, 1000, 1, 0)]
        public void TestServerChange(int x, int y, int z, int mapID)
        {
            var p = new Point3D(x, y, z);
            var map = Map.Maps[mapID];
            var expected = new ServerChange(p, map).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendServerChange(p, map);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestSkillsUpdate()
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var skills = m.Skills;
            m.Skills[Utility.RandomSkill()].BaseFixedPoint = 1000;

            var expected = new SkillUpdate(skills).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendSkillsUpdate(skills);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(255)]
        public void TestSequence(byte num)
        {
            var expected = new Sequence(num).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendSequence(num);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(SkillName.Alchemy, 0, 1)]
        [InlineData(SkillName.Archery, 10, 1000)]
        [InlineData(SkillName.Begging, 100000, 1000)]
        public void TestSkillChange(SkillName skillName, int baseFixedPoint, int capFixedPoint)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var skill = m.Skills[skillName];
            skill.BaseFixedPoint = baseFixedPoint;
            skill.CapFixedPoint = capFixedPoint;

            var expected = new SkillChange(skill).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendSkillChange(skill);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("This is a URL, I promise")]
        [InlineData("https://www.modernuo.com")]
        public void TestLaunchBrowser(string uri)
        {
            var expected = new LaunchBrowser(uri).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendLaunchBrowser(uri);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0x1000u, 1000, 100, 10, 0x2000u, 1125, 125, 5, 0x384, 1024, 25)]
        public void TestDragEffect(
            uint srcSerial, int srcX, int srcY, int srcZ,
            uint trgSerial, int trgX, int trgY, int trgZ,
            int itemId, int hue, int amount
        )
        {
            var src = new Entity(srcSerial, new Point3D(srcX, srcY, srcZ), null);
            var targ = new Entity(trgSerial, new Point3D(trgX, trgY, trgZ), null);

            var expected = new DragEffect(src, targ, itemId, hue, amount).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendDragEffect(
                src.Serial, src.Location,
                targ.Serial, targ.Location,
                itemId, hue, amount
            );

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public void TestSeasonChange(int season, bool playSound)
        {
            var expected = new SeasonChange(season, playSound).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendSeasonChange((byte)season, playSound);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0x1024u, "Test Title", true, true)]
        [InlineData(0x1024u, "Test Title", false, true)]
        [InlineData(0x1024u, "Test Title", true, false)]
        public void TestDisplayPaperdoll(uint m, string title, bool warmode, bool canLift)
        {
            var expected = new DisplayPaperdoll(m, title, warmode, canLift).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendDisplayPaperdoll(m, title, warmode, canLift);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(MusicName.Approach)]
        [InlineData(MusicName.Combat1)]
        [InlineData(MusicName.ValoriaShips)]
        public void TestPlayMusic(MusicName music)
        {
            var expected = new PlayMusic(music).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendPlayMusic(music);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(10, 1, "Some text")]
        [InlineData(100, 10, "Some more text")]
        public void TestScrollMessage(int type, int tip, string text)
        {
            var expected = new ScrollMessage(type, tip, text).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendScrollMessage(type, tip ,text);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(14, 10, 05)]
        public void TestCurrentTime(int hour, int minute, int second)
        {
            var date = new DateTime(2020, 1, 1, hour, minute, second);
            var expected = new CurrentTime(date).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendCurrentTime(date);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(1000, 10, 1)]
        public void TestPathfindMessage(int x, int y, int z)
        {
            var p = new Point3D(x, y, z);
            var expected = new PathfindMessage(p).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendPathfindMessage(p);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestPingAck(byte ping)
        {
            var expected = new PingAck(ping).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendPingAck(ping);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestClearAbility()
        {
            var expected = new ClearWeaponAbility().Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendClearWeaponAbility();

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(0xFF01)]
        public void TestDisplayHuePicker(int itemID)
        {
            var huePicker = new HuePicker(itemID);

            var expected = new DisplayHuePicker(huePicker).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendDisplayHuePicker(huePicker.Serial, huePicker.ItemID);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }
    }
}
