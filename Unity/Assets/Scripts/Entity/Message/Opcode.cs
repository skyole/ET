﻿namespace Model
{
	public enum Opcode: ushort
	{
        #region 客户端 1 +

	    // 客户端开始编号从 1 开始递增
        ClinetNoStart = 1,

        #region Demo自带

        ARequest,
        AResponse,
        AActorMessage,
        AActorRequest,
        AActorResponse,
        ActorRequest,
        ActorResponse,
        ActorRpcRequest,
        ActorRpcResponse,
        AFrameMessage,
        FrameMessage,
        C2R_Login,
        R2C_Login,
        R2C_ServerLog,
        C2G_LoginGate,
        G2C_LoginGate,
        C2G_EnterMap,
        G2C_EnterMap,
        C2M_Reload,
        M2C_Reload,
        C2R_Ping,
        R2C_Ping,

        Actor_Test,
        Actor_TestRequest,
        Actor_TestResponse,
        Actor_TransferRequest,
        Actor_TransferResponse,
        Frame_ClickMap,
        Actor_CreateUnits,


        #endregion

        /// <summary>
        /// 退出
        /// </summary>
	    Quit,

        /// <summary>
        /// 请求适配
        /// </summary>
        StartMatchRt,

        /// <summary>
        /// 适配结果
        /// </summary>
	    StartMatchRe,

        /// <summary>
        /// 修改游戏模式
        /// </summary>
	    ChangeGameMode,

        /// <summary>
        /// 抢地主
        /// </summary>
	    GrabLordSelect,

       

        #endregion

        #region 服务端 10000 +

        // 服务端开始编号从 10000 开始递增
        ServerNoStart = 10000,

        #region Realm

        /// <summary>
        /// 玩家登陆 ET
        /// </summary>
        LoginRt,

        /// <summary>
        /// 玩家登陆 RE
        /// </summary>
        LoginRe,

        /// <summary>
        /// 玩家注册 RT
        /// </summary>
	    RegisterRt,

        /// <summary>
        /// 玩家注册 RE
        /// </summary>
	    RegisterRe,

        #endregion

        #region Gate

        /// <summary>
        /// 获取授权Key
        /// </summary>
        GetLoginKeyRt,

        /// <summary>
        /// 返回授权Key
        /// </summary>
	    GetLoginKeyRe,

        /// <summary>
        /// 用户信息 请求
        /// </summary>
	    GetUserInfoRt,

        /// <summary>
        /// 用户信息 应答
        /// </summary>
	    GetUserInfoRe,

        /// <summary>
        /// 登录授权 请求
        /// </summary>
	    LoginGateRt,

        /// <summary>
        /// 登录授权 应答
        /// </summary>
	    LoginGateRe,
        
        /// <summary>
        /// 加入适配请求
        /// </summary>
        JoinMatchRt,

        /// <summary>
        /// 加入适配应答
        /// </summary>
        JoinMatchRe,


        #endregion

        #region 网关

        /// <summary>
        /// 用户断开
        /// </summary>
        PlayerDisconnect,

        /// <summary>
        /// RoomKey
        /// </summary>
	    RoomKey,

        /// <summary>
        /// 用户退出 DDZ 
        /// </summary>
	    PlayerQuitDdz,

        /// <summary>
        /// 用户退出 Macth
        /// </summary>
        PlayerQuitMacth,


        #endregion

        #region Match

        /// <summary>
        /// 匹配成功
        /// </summary>
	    MatchSuccess,

        #endregion

        #region ddz

        /// <summary>
        /// 玩家退出房间
        /// </summary>
        GamerQuitRoom,

        /// <summary>
        /// 创建房间 请求
        /// </summary>
	    CreateRoomRt,

        /// <summary>
        /// 创建房间 结果
        /// </summary>
	    CreateRoomRe,

        /// <summary>
        /// 玩家弃牌
        /// </summary>
	    Discard,

        /// <summary>
        /// 先出牌消息
        /// </summary>
	    AuthorityPlayCard,

        /// <summary>
        /// 获取加入房间KEY
        /// </summary>
	    GetJoinRoomKeyRt,

        /// <summary>
        /// 返回加入房间KEY
        /// </summary>
	    GetJoinRoomKeyRe,

        /// <summary>
        /// 游戏倍数
        /// </summary>
	    GameMultiples,

        /// <summary>
        /// 地主牌
        /// </summary>
	    SelectLord,

        /// <summary>
        /// 同步游戏状态
        /// </summary>
	    SyncRoomState,

        /// <summary>
        /// 玩家钱太少
        /// </summary>
	    GamerMoneyLess,

        /// <summary>
        /// 先手玩家
        /// </summary>
	    SelectAuthority,

        /// <summary>
        /// 游戏开始
        /// </summary>
	    GameStart,

        /// <summary>
        /// 玩家出牌
        /// </summary>
	    PlayCardsRt,

        /// <summary>
        /// 玩家出牌
        /// </summary>
	    PlayCardsRe,

        /// <summary>
        /// 广播玩家出牌消息
        /// </summary>
	    GamerPlayCards,

        /// <summary>
        /// 游戏结束
        /// </summary>
	    Gameover,

        /// <summary>
        /// 加入房间 RT
        /// </summary>
	    PlayerJoinRoomRt,

        /// <summary>
        /// 加入房间RE
        /// </summary>
	    PlayerJoinRoomRe,

        /// <summary>
        /// 玩家进入
        /// </summary>
	    GamerEnter,

        /// <summary>
        /// 玩家退出
        /// </summary>
	    GamerOut,

        /// <summary>
        /// 玩家准备
        /// </summary>
	    PlayerReady,

        /// <summary>
        /// 玩家重连
        /// </summary>
	    PlayerReconnect,

        /// <summary>
        /// 广播重连消息
        /// </summary>
	    GamerReenter,

        /// <summary>
        /// 玩家重新连接
        /// </summary>
	    GamerReconnect,

        /// <summary>
        /// 提示出牌 RT
        /// </summary>
	    PromptRt,

        /// <summary>
        /// 提示出牌 RE
        /// </summary>
	    PromptRe,

        #endregion

        #region Demo自带

        G2G_LockRequest,
        G2G_LockResponse,
        G2G_LockReleaseRequest,
        G2G_LockReleaseResponse,

        M2A_Reload,
        A2M_Reload,

        DBSaveRequest,
        DBSaveResponse,
        DBQueryRequest,
        DBQueryResponse,
        DBSaveBatchResponse,
        DBSaveBatchRequest,
        DBQueryBatchRequest,
        DBQueryBatchResponse,
        DBQueryJsonRequest,
        DBQueryJsonResponse,

        ObjectAddRequest,
        ObjectAddResponse,
        ObjectRemoveRequest,
        ObjectRemoveResponse,
        ObjectLockRequest,
        ObjectLockResponse,
        ObjectUnLockRequest,
        ObjectUnLockResponse,
        ObjectGetRequest,
        ObjectGetResponse,

        R2G_GetLoginKey,
        G2R_GetLoginKey,

        G2M_CreateUnit,
        M2G_CreateUnit,

        M2M_TrasferUnitRequest,
        M2M_TrasferUnitResponse,

        #endregion


        #endregion

    }
}
