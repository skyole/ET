namespace Model
{
	public static class ErrorCode
	{
		public const int ERR_Success = 0;

		public const int ERR_NotFoundActor = 1;

        /// <summary>
        /// 账号在线
        /// </summary>
	    public const int ErrAccountOnline = 10;
        /// <summary>
        /// 账号已经注册
        /// </summary>
	    public const int ErrAccountAlreadyRegister = 11;
        /// <summary>
        /// 查询用户信息错误
        /// </summary>
	    public const int ErrQueryUserInfoError = 12;
        /// <summary>
        /// 加入房间失败
        /// </summary>
	    public const int ErrJoinRoomError = 13;
        public const int ErrStartMatchError = 14;
	    public const int ErrUserMoneyLessError = 15;
        /// <summary>
        /// 玩家出牌错误
        /// </summary>
	    public const int ErrPlayCardError = 16;

        public const int ERR_RpcFail = 101;
		public const int ERR_AccountOrPasswordError = 102;
		public const int ERR_ConnectGateKeyError = 103;
		public const int ERR_ReloadFail = 104;
		public const int ERR_NotFoundUnit = 105;
		public const int ERR_ActorLocationNotFound = 106;
		public const int ERR_SessionActorError = 107;
		public const int ERR_ActorError = 108;
	}
}