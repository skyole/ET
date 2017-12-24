namespace Model
{
	public static class ErrorCode
	{
		public const int ERR_Success = 0;

		public const int ERR_NotFoundActor = 1;

        /// <summary>
        /// �˺�����
        /// </summary>
	    public const int ErrAccountOnline = 10;
        /// <summary>
        /// �˺��Ѿ�ע��
        /// </summary>
	    public const int ErrAccountAlreadyRegister = 11;
        /// <summary>
        /// ��ѯ�û���Ϣ����
        /// </summary>
	    public const int ErrQueryUserInfoError = 12;
        /// <summary>
        /// ���뷿��ʧ��
        /// </summary>
	    public const int ErrJoinRoomError = 13;
        public const int ErrStartMatchError = 14;
	    public const int ErrUserMoneyLessError = 15;
        /// <summary>
        /// ��ҳ��ƴ���
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