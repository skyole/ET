using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ProtoBuf;

namespace Model
{

    #region 原有

    [BsonKnownTypes(typeof(AActorMessage))]
    public abstract partial class AMessage
    {
    }

    [BsonKnownTypes(typeof(AActorRequest))]
    [ProtoInclude((int)Opcode.C2R_Login, typeof(C2R_Login))]
    [ProtoInclude((int)Opcode.C2G_LoginGate, typeof(C2G_LoginGate))]
    [ProtoInclude((int)Opcode.C2G_EnterMap, typeof(C2G_EnterMap))]
    [ProtoInclude((int)Opcode.C2R_Ping, typeof(C2R_Ping))]
    public abstract partial class ARequest : AMessage
    {
    }

    [BsonKnownTypes(typeof(AActorResponse))]
    [ProtoInclude((int)Opcode.R2C_Login, typeof(R2C_Login))]
    [ProtoInclude((int)Opcode.G2C_LoginGate, typeof(G2C_LoginGate))]
    [ProtoInclude((int)Opcode.G2C_EnterMap, typeof(G2C_EnterMap))]
    [ProtoInclude((int)Opcode.R2C_Ping, typeof(R2C_Ping))]
    public abstract partial class AResponse : AMessage
    {
    }

    [BsonKnownTypes(typeof(Actor_Test))]
    [BsonKnownTypes(typeof(AFrameMessage))]
    [BsonKnownTypes(typeof(Actor_CreateUnits))]
    [BsonKnownTypes(typeof(FrameMessage))]
    [ProtoInclude((int)Opcode.FrameMessage, typeof(FrameMessage))]
    [ProtoInclude((int)Opcode.AFrameMessage, typeof(AFrameMessage))]
    [ProtoInclude((int)Opcode.Actor_CreateUnits, typeof(Actor_CreateUnits))]
    public abstract partial class AActorMessage : AMessage
    {
    }

    [BsonKnownTypes(typeof(Actor_TestRequest))]
    [BsonKnownTypes(typeof(Actor_TransferRequest))]
    public abstract partial class AActorRequest : ARequest
    {
    }

    [BsonKnownTypes(typeof(Actor_TestResponse))]
    [BsonKnownTypes(typeof(Actor_TransferResponse))]
    public abstract partial class AActorResponse : AResponse
    {
    }

    /// <summary>
    /// 帧消息，继承这个类的消息会经过服务端转发
    /// </summary>
    [ProtoInclude((int)Opcode.Frame_ClickMap, typeof(Frame_ClickMap))]
    [BsonKnownTypes(typeof(Frame_ClickMap))]
    public abstract partial class AFrameMessage : AActorMessage
    {
    }

    [ProtoContract]
    [Message(Opcode.C2R_Login)]
    public class C2R_Login : ARequest
    {
        [ProtoMember(1)]
        public long Account;

        [ProtoMember(2)]
        public string Password;
    }

    [ProtoContract]
    [Message(Opcode.R2C_Login)]
    public class R2C_Login : AResponse
    {
        [ProtoMember(1)]
        public string Address { get; set; }

        [ProtoMember(2)]
        public long Key { get; set; }
    }

    [ProtoContract]
    [Message(Opcode.C2G_LoginGate)]
    public class C2G_LoginGate : ARequest
    {
        [ProtoMember(1)]
        public long Key;
    }

    [ProtoContract]
    [Message(Opcode.G2C_LoginGate)]
    public class G2C_LoginGate : AResponse
    {
        [ProtoMember(1)]
        public long PlayerId;
    }


    [Message(Opcode.Actor_Test)]
    public class Actor_Test : AActorMessage
    {
        public string Info;
    }

    [Message(Opcode.Actor_TestRequest)]
    public class Actor_TestRequest : AActorRequest
    {
        public string request;
    }

    [Message(Opcode.Actor_TestResponse)]
    public class Actor_TestResponse : AActorResponse
    {
        public string response;
    }


    [Message(Opcode.Actor_TransferRequest)]
    public class Actor_TransferRequest : AActorRequest
    {
        public int MapIndex;
    }

    [Message(Opcode.Actor_TransferResponse)]
    public class Actor_TransferResponse : AActorResponse
    {
    }

    [ProtoContract]
    [Message(Opcode.C2G_EnterMap)]
    public class C2G_EnterMap : ARequest
    {
    }

    [ProtoContract]
    [Message(Opcode.G2C_EnterMap)]
    public class G2C_EnterMap : AResponse
    {
        [ProtoMember(1)]
        public long UnitId;
        [ProtoMember(2)]
        public int Count;
    }

    [ProtoContract]
    public class UnitInfo
    {
        [ProtoMember(1)]
        public long UnitId;
        [ProtoMember(2)]
        public int X;
        [ProtoMember(3)]
        public int Z;
    }

    [ProtoContract]
    [Message(Opcode.Actor_CreateUnits)]
    public class Actor_CreateUnits : AActorMessage
    {
        [ProtoMember(1)]
        public List<UnitInfo> Units = new List<UnitInfo>();
    }

    public struct FrameMessageInfo
    {
        public long Id;
        public AMessage Message;
    }

    [ProtoContract]
    [Message(Opcode.FrameMessage)]
    public class FrameMessage : AActorMessage
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public List<AFrameMessage> Messages = new List<AFrameMessage>();
    }

    [ProtoContract]
    [Message(Opcode.Frame_ClickMap)]
    public class Frame_ClickMap : AFrameMessage
    {
        [ProtoMember(1)]
        public int X;
        [ProtoMember(2)]
        public int Z;
    }

    [Message(Opcode.C2M_Reload)]
    public class C2M_Reload : ARequest
    {
        public AppType AppType;
    }

    [Message(Opcode.M2C_Reload)]
    public class M2C_Reload : AResponse
    {
    }

    [ProtoContract]
    [Message(Opcode.C2R_Ping)]
    public class C2R_Ping : ARequest
    {
    }

    [ProtoContract]
    [Message(Opcode.R2C_Ping)]
    public class R2C_Ping : AResponse
    {
    }

    #endregion

    #region Realm

    /// <summary>
    /// 玩家登陆 RT
    /// </summary>
    [Message(Opcode.LoginRt)]
    public class LoginRt : ARequest
    {
        public string Account;
        public string Password;
    }

    /// <summary>
    /// 玩家登陆 RE
    /// </summary>
    [Message(Opcode.LoginRe)]
    public class LoginRe : AResponse
    {
        public long Key;
        public string Address;
    }

    /// <summary>
    /// 玩家注册 RT
    /// </summary>
    [Message(Opcode.RegisterRt)]
    public class RegisterRt : ARequest
    {
        public string Account;
        public string Password;
    }

    /// <summary>
    /// 玩家注册 RE
    /// </summary>
    [Message(Opcode.RegisterRe)]
    public class RegisterRe : AResponse
    {

    }


    #endregion

    #region Gate

    /// <summary>
    /// 房间Key
    /// </summary>
    [Message(Opcode.RoomKey)]
    public class RoomKey : AActorMessage
    {
        public long Key;
    }

    /// <summary>
    /// 玩家退出 请求
    /// </summary>
    [Message(Opcode.Quit)]
    public class Quit : AMessage
    {
        public long PlayerId;
    }

    #endregion

    #region Match

    /// <summary>
    /// 房间适配 请求
    /// </summary>
    [Message(Opcode.StartMatchRt)]
    public class StartMatchRt : ARequest
    {
        public long PlayerId;
        public RoomLevel Level;
    }

    /// <summary>
    /// 房间适配 应答
    /// </summary>
    [Message(Opcode.StartMatchRe)]
    public class StartMatchRe : AResponse
    {

    }

    #endregion

    #region DDZ

    /// <summary>
    /// 更换游戏模式
    /// </summary>
    [Message(Opcode.ChangeGameMode)]
    public class ChangeGameMode : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 玩家弃牌
    /// </summary>
    [Message(Opcode.Discard)]
    public class Discard : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 先出牌消息
    /// </summary>
    [Message(Opcode.AuthorityPlayCard)]
    public class AuthorityPlayCard : AActorMessage
    {
        public long PlayerId;
        public bool IsFirst;
    }

    /// <summary>
    /// 抢地主
    /// </summary>
    [Message(Opcode.GrabLordSelect)]
    public class GrabLordSelect : AActorMessage
    {
        public long PlayerId;
        public bool IsGrab;
    }

    /// <summary>
    /// 游戏倍数 
    /// </summary>
    [Message(Opcode.GameMultiples)]
    public class GameMultiples : AActorMessage
    {
        public int Multiples;
    }

    /// <summary>
    /// 广播地主牌
    /// </summary>
    [Message(Opcode.SelectLord)]
    public class SelectLord : AActorMessage
    {
        public long PlayerId;
        public Card[] LordCards;
    }

    /// <summary>
    /// 玩家钱太少
    /// </summary>
    [Message(Opcode.GamerMoneyLess)]
    public class GamerMoneyLess : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 广播先手玩家
    /// </summary>
    [Message(Opcode.SelectAuthority)]
    public class SelectAuthority : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    [Message(Opcode.GameStart)]
    public class GameStart : AActorMessage
    {
        public Card[] GamerCards;
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, int> GamerCardsNum;
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    [Message(Opcode.PlayCardsRt)]
    public class PlayCardsRt : AActorRequest
    {
        public long PlayerId;
        public Card[] Cards;
    }

    /// <summary>
    /// 玩家出牌应答
    /// </summary>
    [Message(Opcode.PlayCardsRe)]
    public class PlayCardsRe : AActorResponse
    {

    }

    /// <summary>
    /// 广播玩家出牌消息
    /// </summary>
    [Message(Opcode.GamerPlayCards)]
    public class GamerPlayCards : AActorMessage
    {
        public long PlayerId;
        public Card[] Cards;
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    [Message(Opcode.Gameover)]
    public class Gameover : AActorMessage
    {
        public Identity Winner;
        public long BasePointPerMatch;
        public int Multiples;
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, long> GamersScore;
    }

    /// <summary>
    /// 加入房间 RT
    /// </summary>
    [Message(Opcode.PlayerJoinRoomRt)]
    public class PlayerJoinRoomRt : AActorRequest
    {
        public long Key;
    }

    /// <summary>
    /// 加入房间 RE
    /// </summary>
    [Message(Opcode.PlayerJoinRoomRe)]
    public class PlayerJoinRoomRe : AActorResponse
    {

    }

    /// <summary>
    /// 玩家信息
    /// </summary>
    public class GamerInfo
    {
        public long PlayerId;
        public long UserId;
        public bool IsReady;
    }

    /// <summary>
    /// 玩家进入
    /// </summary>
    [Message(Opcode.GamerEnter)]
    public class GamerEnter : AActorMessage
    {
        public long RoomId;
        public GamerInfo[] GamersInfo;
    }

    /// <summary>
    /// 玩家退出
    /// </summary>
    [Message(Opcode.GamerOut)]
    public class GamerOut : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 玩家准备
    /// </summary>
    [Message(Opcode.PlayerReady)]
    public class PlayerReady : AActorMessage
    {
        public long PlayerId;
    }

    /// <summary>
    /// 玩家重连
    /// </summary>
    [Message(Opcode.PlayerReconnect)]
    [BsonIgnoreExtraElements]
    public class PlayerReconnect : AMessage
    {
        public long PlayerId;
        public long UserId;
        public long GateSessionId;
    }

    /// <summary>
    /// 广播重连消息
    /// </summary>
    [Message(Opcode.GamerReenter)]
    public class GamerReenter : AActorMessage
    {
        public long PastId;
        public long NewId;
    }

    /// <summary>
    /// 玩家重新连接
    /// </summary>
    [Message(Opcode.GamerReconnect)]
    public class GamerReconnect : AActorMessage
    {
        public long PlayerId;
        public int Multiples;
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, Identity> GamersIdentity;
        public Card[] LordCards;
        public KeyValuePair<long, Card[]> DeskCards;
    }

    /// <summary>
    /// 提示出牌 RT
    /// </summary>
    [Message(Opcode.PromptRt)]
    public class PromptRt : AActorRequest
    {
        public long PlayerId;
    }

    /// <summary>
    /// 提示出牌 RE
    /// </summary>
    [Message(Opcode.PromptRe)]
    public class PromptRe : AActorResponse
    {
        public Card[] Cards;
    }

    #endregion

}