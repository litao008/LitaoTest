using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BinHong.PhoenixCore.Connect
{
    public class GlobalObjects
    {
        public class TestConnection
        {
            public string coreName = "";
            public string remoteIP = "";
            public string toConnect = "";
            public string remotePort = "";
            public string linkStatus = "";
            public string loginStatus = "";
        }
        #region 控制进度条参数
        /// <summary>
        /// 进度增长次数
        /// </summary>
        public static int barIncreaseNum = 0;
        #endregion
       
        public static List<TestConnection> TestConnSet = new List<TestConnection>();
        //public static AutoResetEvent PullMsgRecvEvent = new AutoResetEvent(false);
        public class ListViewData
        {
            internal string CoreName;
            internal string strport;
            internal string conectedWay;//连接方式
        }
        public class SearchTxt
        {
            internal int Searchline;//行号
            internal int SearchNo;//位置
            internal string SearchWord;//该行内容
            internal string SearchWorduse;//搜索的内容
        }
        //internal static Dictionary<int, ListViewData> baseListView = new Dictionary<int, ListViewData>();
        internal static List<ListViewData> baseListView = new List<ListViewData>();
        public static byte[] PullMsgBuffer = new byte[65536];
        public static Queue PushLogMsgQueue = new Queue(100);
        public static Queue PushAlarmMsgQueue = new Queue(100);
        public static Queue PushMemoryMsgQueue = new Queue(100);
        public static object syncLockPullMsgBuffer = new object();
        public static object syncLockPushMsgQueue = new object();
        public static object syncLockTestConnSet = new object();
        public static object syncLockUserLogDisplay = new object();
        public static List<Task> SystemTaskList = new List<Task>(1000);
        public static SynchronizationContext UiContext = SynchronizationContext.Current;    
        public static BinaryWriter binw_logdetail = null;
        public static BinaryWriter binw_logsummary = null;
        public static BinaryWriter binw_alarm = null;
        public static BinaryWriter binw_memory = null;

        internal static Socket UdpSocket = null;
        
        
        public static int UdpRecvBufferSize = 0;
        public static int LogFileMaxBytesNum = 0;
        public static int SummaryLogFileMaxBytesNum = 0;
        public static int AlarmFileMaxBytesNum = 0;
        public static int MemoryFileMaxBytesNum = 0;
        public static int LogBufferSize = 0;
        public static int LogSummaryBufferSize = 0;
        public static int AlarmBufferSize = 0;
        public static int MemoryBufferSize = 0;
        public static string ConfigFileDir = "";
        public static string BackupFileDir = "";
        public static int LoginTimedOutInMs = 5000; //ms
        public static int RepeatLoginIntervalInMs = 100;//ms
        public static bool IsLoginClearLog = true;
        public static bool IsLoginClearAlarm = true;
        public static bool IsUpdateLogsummaryGrid = true;
        //public static bool IsUpdateLogsummaryGridAfterLogin = true;
        //public static bool IsUpdateAlarmGridAfterLogin = true;
        //public static bool IsUpdateAfterUpdate = false;
        //public static bool IsUpdatecoselUpdate = false;
        public static int LogSummaryGridUpdateInterval = 0;
        public static int LogSummaryGridValidDuration = 0;
        public static int MaximumDisplayLogSummaryItem = 10000;
        //
    }
    public enum EState
    {
        EState_None = 0,
        EState_Logo,
        EState_LogIn,
        EState_PCAGENT_MAIN_MSGTYPE_PPCCONNECT_REQ,  
        EState_PCAGENT_MAIN_MSGTYPE_CONNECT_REQ,
        EState_WaitPCAGENT_MAIN_MSGTYPE_CONNECT_REQ,
        EState_DAL_AGENT_SUB_MSGTYPE_CALLTFUNC_RSP,
        EState_DAL_AGENT_SUB_MSGTYPE_TCPUPLOAD_RSP,
        EState_DAL_AGENT_SUB_MSGTYPE_TCPHEART_RSP,
        EState_DAL_AGENT_SUB_MSGTYPE_TCPDOWNLOAD_RSP,//上传至PC
        EState_WaitDAL_AGENT_SUB_MSGTYPE_CALLTFUNC_RSP,
        EState_OSP_STRU_CONSOLE_REDIRECT,
        EState_WaitOSP_STRU_CONSOLE_REDIRECT,
        EState_OSP_STRU_CONSOLE_MESSAGE,
        EState_WaitOSP_STRU_CONSOLE_MESSAGE,
        EState_DAL_AGENT_SUB_MSGTYPE_TCPUNLOAD_RSP,
    }

    public enum MsgType
    {
        /* Osp Agent 和 PC 交互时的消息主类型定义                            */
        /* PC --> Agent 消息类型                                             */
        PCAGENT_MAIN_MSGTYPE_CONNECT_REQ = 1,                /* PC端发起的连接请求消息             */
        PCAGENT_MAIN_MSGTYPE_CALLTFUNC_REQ = 2,              /* PC端发起调用调测接口信息时的消息   */
        PCAGENT_MAIN_MSGTYPE_REDIRECT_CONSOLE = 3,           /* PC端发起的重定向OSP控制台的消息    */
        PCAGENT_MAIN_MSGTYPE_CHANGE_MEM = 4,                 /* PC端发起的更改内存内容的消息       */
        PCAGENT_MAIN_MSGTYPE_COMMAND = 5,                   /* 执行控制台命令行消息               */
    }
    public enum MsgType1
    {
        /* Osp Agent 和 PC 交互时的消息子类型定义                            */
        OSP_AGENT_SUB_MSGTYPE_NORMAL = 1,
        OSP_AGENT_SUB_MSGTYPE_FAIL = 2,
        DAL_AGENT_SUB_MSGTYPE_CONNECT_RSP_TFUNC_NUM = 3,                   /* Agent回复的含调试接口总个数的消息  */
        DAL_AGENT_SUB_MSGTYPE_CONNECT_RSP_TFUNC_SINGLE = 4,                  /* Agent回复的单个调试接口的信息      */
        DAL_AGENT_SUB_MSGTYPE_CALLTFUNC_RSP_MSG1 = 5,                   /* Agent回复调用调试接口的Msg1        */
        DAL_AGENT_SUB_MSGTYPE_CALLTFUNC_RSP_MSG2 = 6,                   /* Agent回复调用调试接口的Msg2        */
        DAL_AGENT_SUB_MSGTYPE_CHANGE_MEM_RSP = 7,                  /* Agent回复修改内存的消息            */
        DAL_AGENT_SUB_MSGTYPE_CONSOLEMSG_RSP        =       8 ,
        PCAGENT_MAIN_MSGTYPE_LOG_UPLOAD = 10,
    }
    public enum MsgType2
    {
        /* 显示信息缓冲区结构类型宏定义                                      */
        OSP_BUFFER_TYPE_NULL = 0,             /* 空白缓冲区                         */
        OSP_BUFFER_TYPE_TABLE = 1,            /* 表型缓冲区                         */
        OSP_BUFFER_TYPE_TREE = 2,             /* 树型缓冲区                         */
        OSP_BUFFER_TYPE_PROP = 3,             /* 属性表缓冲区                       */
        OSP_BUFFER_TYPE_MEM = 4,             /* 内存型缓冲区                       */
        OSP_BUFFER_TYPE_FRET = 5,             /* 函数返回值缓冲区                   */
    }
    public enum MsgType3
    {
        /* 显示信息缓冲区结构类型宏定义                                      */
        OSP_BUFFER_TYPE_NULL = 0,                    /* 空白缓冲区                         */
        OSP_BUFFER_TYPE_TABLE = 1,                    /* 表型缓冲区                         */
        OSP_BUFFER_TYPE_TREE = 2,                    /* 树型缓冲区                         */
        OSP_BUFFER_TYPE_PROP = 3,                    /* 属性表缓冲区                       */
        OSP_BUFFER_TYPE_MEM = 4,                    /* 内存型缓冲区                       */
        OSP_BUFFER_TYPE_FRET = 5,                    /* 函数返回值缓冲区                   */
    }
    public enum MsgType4
    {
        OSP_OK = 0,                             /* 成功                               */
        OSP_ERROR = (-1),                             /* 失败                               */
    }
    /*-----------表格显示方式------------*/
    public enum MsgType5
    {
        OSP_TABLE_SHOW_STR = 1,                           /* 按照字符串显示                     */
        OSP_TABLE_SHOW_SDEC = 2,                           /* 按照有符号十进制整型显示           */
        OSP_TABLE_SHOW_UDEC = 3,                           /* 按照无符号十机制整型显示           */
        OSP_TABLE_SHOW_HEX = 4,                           /* 按照十六进制整型显示               */
    }
    public enum MsgType6
    {
        OSP_TABLE_SHOW_STR = 1,                              /* 按照字符串显示                     */
        OSP_TABLE_SHOW_SDEC = 2,                             /* 按照有符号十进制整型显示           */
        OSP_TABLE_SHOW_UDEC = 3,                              /* 按照无符号十机制整型显示           */
        OSP_TABLE_SHOW_HEX = 4,                                /* 按照十六进制整型显示               */
    }
    public enum MsgType7
    {
        /*---------调试HOOK参数类型----------*/
        OSP_ARG_TYPE_INT = 1,                                   /* 整型                               */
        OSP_ARG_TYPE_STR = 2,                                   /* 字符串                             */
        OSP_ARG_TYPE_BOOL = 3,                                   /* 布尔型                             */
        OSP_ARG_TYPE_FILE_CONTENT = 4,                                   /* 文件内容                           */
        OSP_ARG_TYPE_FILE = 5,                                   /* 文件类型                           */
    }
    public enum MsgType8
    {
        // 会话对段处理器类型宏定义
        PROC_TYPE_PPC = 0,
        PROC_TYPE_DSP = 1,
    }
    /*---------DAL message header structure---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_DAL_STRU_MSGHEAD
    {
        public Int16 MsgId;                                     /** Message ID */
        public Int16 MsgSize;                                   /** Message size in byte */
        public Int16 SrcPeId;                                   /** Source Pe descriptor */
        public Int16 DstPeId;                                   /** Destination Pe descriptor */
        public byte MsgOpt;                                    /** Message optional */
        public byte MsgPrio;                                   /** Message priority (0[Lowest]~4[Highest]) */
        public byte TraceId;                                   /** Message trace */
        public byte Reserv;                                    /** Reserved */
    }
    /*--------Agent used msg header structure---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DAL_STRU_AGENT_COMM_MSGHEAD
    {
        public tag_DAL_STRU_MSGHEAD struMsgHead;
        public Int64 SubCmd;
        public Int16 Rsv;
    }
    /*--------Login msg---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_DAL_STRU_AGENT_LOGIN_REQ
    {
        public tag_DAL_STRU_MSGHEAD struAgentHead;
        public Int32 u32RequestId; /** Request sequence id of the current operation */
    }
    /*--------Login ack msg---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_DAL_STRU_AGENT_LOGIN_ACK
    {
        public tag_DAL_STRU_MSGHEAD struAgentHead;
        public Int32 u32ResponseId; /** Response sequence id of the current operation */
        public Int32 u32Status;
    }
    /*--------Logout msg---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_DAL_STRU_AGENT_LOGOUT_REQ
    {
        public tag_DAL_STRU_MSGHEAD struAgentHead;
        public Int32 u32RequestId; /** Request sequence id of the current operation */
    }
    /*--------Logout ack msg---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_DAL_STRU_AGENT_LOGOUT_ACK
    {
        public tag_DAL_STRU_MSGHEAD struAgentHead;
        public Int32 u32ResponseId; /** Response sequence id of the current operation */
        public Int32 u32Status;
    }
    /* PC端和OSP_AGENT交互时的消息头结构定义                             */
    [StructLayout(LayoutKind.Explicit)]
    public struct SeqID
    {
        [FieldOffset(0)]
        public UInt16 u16ReqID;                                                           /* 请求ID                             */
        [FieldOffset(0)]
        public UInt16 u16AckID;                                                           /* 确认ID                             */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_OSPDD_AGENT_COMM_MSGHEAD
    {
        public short s16MsgMainType;                                                         /* 消息主类型                         */
        public SeqID seqid;
        public ushort u16FragFlag;                                                            /* 分段标志                           */
        public ushort u16FragOffset;                                                          /* 段偏移                             */
        public short u16MsgSize;                                                             /* 消息大小                           */
        public short s16MsgSubType;                                                             /* 消息子类型                       */
    }
    /* Client发起的连接请求消息结构定义                                  */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_OSP_STRU_CLIENT_CONNECT_REQ_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                                  /* XXXX消息头                         */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_STRU_CLIENT_REQ_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                   /* XXXX消息头 */
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        //public char[] ucMsgBuf;             /* 消息体内容 */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    /* Client发起的调用调测接口请求消息结构定义                          */
    public struct tag_OSP_STRU_CLIENT_CALLTESTFUNC_REQ_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                          /* XXXX消息头                         */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] s8SubSystemName;                  /* 调测功能所属子系统名称             */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] s8ModuleName;                     /* 调测功能所属模块名称               */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] s8TestFuncDescp;                  /* 调测功能描述信息                   */
        public short s16TestFuncID;                     /* 测试函数ID                         */
        public short s16ArgLen;                                        /* 参数内容的大小                     */  
        public Int32 u32DelayMSeconds;                                  /* 等待时间长度                       */          
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8000)]
        public Byte[] s8ArgBuf;                   /* 参数缓冲区                         */                       
    }
    /* Agent 回复 Client 连接请求的消息结构定义                          */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_OSP_STRU_CLIENT_CONNECT_ACK_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                                  /* XXXX消息头                         */
        public uint u32DbgFuncNum;                                    /* 调测接口个数                       */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    /* 更改内存的请求消息 */
    public struct tag_OSP_STRU_CHANGE_MEM_ITEM
    {
        public uint u32MemAddr;
        public uint u32MemSize;
        public uint u32MemContent;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct tag_OSP_STRU_CHANGE_MEM_REQ_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;
        public uint u32ItemCnt;
        public tag_OSP_STRU_CHANGE_MEM_ITEM struChangeMemItem;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    /* 更改内存的回复消息 */
    public struct tag_OSP_STRU_CHANGE_MEM_ACK_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OSP_STRU_CLIENT_GETDBGFUNC_ACK_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                            /* XXXX消息头                         */
        public OSP_REPORT_TESTFUNC_INFO testFuncInfo;                                    /* 调测接口信息数组                   */
    }

    /* Agent 回复 Client 连接请求的消息结构定义*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct OSP_STRU_CLIENT_CONNECT_ACK_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;           /* XXXX消息头                         */
        public UInt32 u32DbgFuncNum;                                    /* 调测接口个数                       */
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct OSP_STRU_AGENT_FAIL_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;    /* XXXX消息头                         */
        public string s8FailDescp;                               /* 失败原因描述                       */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct OSP_REPORT_TESTFUNC_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] s8SubSystemName;                  /* 调测功能所属子系统名称             */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] s8ModuleName;                     /* 调测功能所属模块名称               */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] s8TestFuncDescp;                  /* 调测功能名称                       */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] s8FuncTipInfo;                    /* 调测功能提示信息                   */
        public short s16TestFuncID;                    /* 调测功能ID                         */
        public short s16ArgNum;
        public Int32 u32DelayMSeconds;                 /* PC端等待延迟时间                   */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =8)]
        public ARG_INFO[] arg_infoList;                      /* 参数信息数组                       */
    }
    /* Agent回复Client端调测接口信息时需上报的调测接口信息结构定义       */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ARG_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] s8ArgDescp;                      /* 调测功能参数名称                   */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] s8ArgTipInfo;                    /* 参数的提示信息                     */
        public Int32 u32ArgSize;                      /* 调测功能参数字节数                 */
        public Int32 u32ArgType;
    }
    /*参数列表结构体*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct OSP_NIRECVDMASG_INFO
    {
        public OSP_STRU_CLIENT_GETDBGFUNC_ACK_MSG struClientGetDbgFuncAckMsg;
        public OSP_STRU_CLIENT_CONNECT_ACK_MSG struClientConnectAckMsg;
        public OSP_STRU_AGENT_FAIL_MSG struAgentFailMsg;
        public byte[] cRecvdBuf;
    }
    /* Agent回复Client调测接口请求消息结构定义1                           */
    [StructLayout(LayoutKind.Explicit)]
    public struct UnMisk
    {
        [FieldOffset(0)]
        public Int32 u32FragmentNum;                                                            /* 显示缓冲区分段个数                 */
        [FieldOffset(0)]
        public Int32 u32FRet;                                                           /* 函数返回值                         */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_CLIENT_CALLTESTFUNC_ACK_MSG1
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                          /* XXXX消息头                         */
        public UInt32 u32MainBufType;                                /* 显示信息缓冲区主类型               */
        public UnMisk unMisk;
    }
    /* Agent回复Client调测接口请求消息结构定义2                           */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_CLIENT_CALLTESTFUNC_ACK_MSG2
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                          /* XXXX消息头                         */
        public tag_OSP_STRU_SUBSHOWBUF struSubShowBuf;                                /* 子显示缓冲区                       */       
    }
    /* 子显示缓冲区类型结构                                              */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_SUBSHOWBUF
    {
        public UInt32 u32ValidDataLen;                             /* 有效数据长度                       */
        public Int32 pNext;                                      /* 指向下一个子缓冲区的指针           */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7004)]
        public byte[] s8ShowInfoBuf;                  /* 显示结构缓冲区                     */
    }
    // 可测性接口函数调用后, 显示ShowBuf时所用事件结构类型
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct STRU_SHOW_BUFFER_MSG
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] cTabItemName;         // TabItem的名称     
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] cTabItemTipName;                       // TabItem的提示信息 
        public int nBufMainType;                               // 缓冲区主类型      
        public int nTabItemId;                                 // TabItemId         
        public uint nShowBufSize;                               // 显示缓冲区大小    
        public byte[] pShowBuf;                                   // 显示缓冲区        
    }
    /* 表的列属性结构定义                                                */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STUR_COLMATTR
    {
        public Int32 u32StoredSize;                                                          /* 存储宽度                           */
        public Int32 u32ShowType;                                                            /* 显示类型                           */
        public string ps8ColumnName;                                                         /* 列名称指针                         */
        public short s32ShowWidth;                                                           /* 列的显示宽度                       */
    }

    /* 解析表型显示信息缓冲区时所需控制结构                              */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_PARSE_TABLE_BUF_CB
    {
        //s8*  pTableBuf;                                                             /* 表形缓冲区起始指针                 */
        public UInt32 u32ColumnNum;                                                          /* 表型结构的列数                     */
        public UInt32 u32LineNum;                                                            /* 表型缓冲区的行数                   */
        public string ps8TableName;                                                          /* 表名称字符串指针                   */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public tag_OSP_STUR_COLMATTR[] strGameTitles;
        //tag_OSP_STUR_COLMATTR struColmAttrArray[OSP_MAX_TABLE_COLMNUM];
    }

    /* Client传来的OSP控制台重定向消息结构定义 */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_CONSOLE_REDIRECT_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                                  /* XXXX消息头                         */
        public tag_OSP_STRU_SOCKADDR viewConsoleAddr;                                       /* View上用于接收控制台消息的sock地址 */
        public Int32 u32DirectToView;                                       /* 是否重定向到View的标志             */
    }

    /*--------SOCEKT地址数据结构---------*/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct tag_OSP_STRU_SOCKADDR
    {
        public byte u8SinLen;                                                               /* 地址长度                           */
        public byte u8SinFamily;                                                            /* 协议族                             */
        public Int16 u16SinPort;                                                             /* 端口                               */
        public Int32 u32SinAddr;                                                             /* IP地址                             */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    /* Client传来的OSP控制台命令消息结构定义 */
    struct tag_OSP_STRU_CONSOLE_COMMAND_MSG
    {
        public tag_OSPDD_AGENT_COMM_MSGHEAD struOspPCCommMsgHead;                                  /* XXXX消息头 */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 416)]
        public byte[] s8CmdBuf;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct NB_TPF_SIG_MSG_LOGON_REQUEST
    {
        public NB_TPF_SIG_MSG_HEAD sigMsgHeader;                           /* logon request signalling message header          */
        /* the following should be user id info */
        /* 1 BYTE : user id parameter option id */
        /* 1 BYTE : user id parameter length    */
        /* x BTYES: user id                     */
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    /* typedef for signalling message header */
    public struct NB_TPF_SIG_MSG_HEAD
    {
         [FieldOffset(0)]
        public byte u8SigMsgFlag;                                       /* signalling message flag                          */
         [FieldOffset(2)]
        public UInt16 u16SigMsgLen;                                       /* signalling message total length(including head)  */
         [FieldOffset(3)]
        public byte u8SigMsgType;                                       /* signalling message type                          */
         [FieldOffset(4)]
        public byte u8SigMsgFunc;                                       /* signalling message function code                 */
         [FieldOffset(5)]
        public byte u8SigMsgBoard;                                      /* signalling message board type&ID                 */
         [FieldOffset(6)]
        public UInt16 u16SigMsgTransID;                                   /* signalling message transaction ID                */
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct NB_TPF_CLT_FILCTL
    {
        public bool bUpldFlag;
        public long wTran;                                      // current transaction
        public string strPcFilNam;                                // pc(local) file name
        public string strTgtFilNam;                               // target(remote) file name
        public long dwFilSiz;                                   // file size
        public bool bLoadFlg;                                   // load flag(only user for file up&load)
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct NB_TPF_SIG_MSG_UPLOAD_REQUEST
    {
        public NB_TPF_SIG_MSG_HEAD sigMsgHeader;                           /* file up&load request signalling message header   */
        public UInt32 u32LoadAndFileSize;                     /* file load flag & size                            */
        /*u16                 u16FileNameLen; */
        /* file name length                                 */
        /* u8               u8FileName[u16FileNameLen]  */
        /* variable length file name                        */
    }
    struct NB_TPF_SIG_MSG_DOWNLOAD_REQUEST
    {
        public NB_TPF_SIG_MSG_HEAD sigMsgHeader;                           /* file download request signalling message header   */
        public UInt32 u32FileSize;                     /* file  size                            */
        /*u16                 u16FileNameLen; */
        /* file name length                                 */
        /* u8               u8FileName[u16FileNameLen]  */
        /* variable length file name                        */
    }
	 #region Log相关--Authored By Feng 2015.8.3
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct DAL_EVENT_LOG_ITEM
    {
        public string sourceId;//the data sourceID
        public UInt32 moduleId; //the ID of the module that the event belongs to 

        public DAL_LOG_TYPE logType;//defined in DAL_LOG_TYPE enumeration

        public UInt32 sequence;//sequence from beginning

        public UInt32 timestamp;//timestamp when event happens 

        //specific attribute for different type of event 
        public UInt32 taskMsgStat;
        public UInt32 statValue;

        //the actual size for output 
        public UInt32 stringSize;
        //public UInt16 paddings;
        
        public string strName;//body - store different content for different event 
        public string revTiem;//人为的添加一个字段:接收到的时间;
    };
    public enum DAL_LOG_TYPE
    {
        TASK_SWITCH = 0,
        MSG_SEND,
        MSG_RECV,
        DURATION_START,
        DURATION_STOP,
        VAR_COUNT,
        INFO,
        WARNING,
        ERROR,
        /* used for number count in this enumeration */
        DAL_LOG_ALL,
    };
    #endregion
}
