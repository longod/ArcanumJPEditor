// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace WebTranslate {
    // thread 対応したいが
    public abstract class TranslateBase {
        private string input = null;
        private string output = null;
        private string url = "about:blank";
        private string ua = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648)"; // IE8
        private System.Net.HttpWebRequest request = null;
        protected static System.Text.Encoding encode = System.Text.Encoding.UTF8;

        public string InputText {
            get { return input; }
            protected set { input = value; }
        }
        public string OutputText {
            get { return output; }
            protected set { output = value; }
        }
        public string URL {
            get { return url; }
            protected set { url = value; }
        }
        public string UserAgent {
            get { return ua; }
            protected set { ua = value; }
        }
        public static System.Text.Encoding Encode {
            get { return encode; }
            //protected set { encode = value; }
        }
        protected System.Net.HttpWebRequest Request {
            get { return request; }
            set { request = value; }
        }

        public string GetResult() {
            string html = GetResponse();
            if( html != null ) {
                string result = GetTrim( html );
                OutputText = result;
                return result;
            }
            return null;
        }

        protected abstract bool Create( string text );
        protected abstract string GetTrim( string html );

        protected static System.Net.HttpWebRequest CreateRequest( string url, string useragent ) {
            try {
                System.Net.HttpWebRequest req = System.Net.WebRequest.Create( url ) as System.Net.HttpWebRequest;
                if( req != null ) {
                    req.UserAgent = useragent;
                }
                return req;
            }
            catch( System.Security.SecurityException e ) {
                //System.Windows.Forms.MessageBox.Show( "アクセス許可がありません" );
                return null;
            }
        }
        private string GetResponse() {
            string html = null;
            if( Request != null ) {
                try {
                    System.Net.HttpWebResponse res = Request.GetResponse() as System.Net.HttpWebResponse;
                    if( res != null ) {
                        System.IO.Stream stream = res.GetResponseStream();
                        if( stream != null ) {
                            System.IO.StreamReader reader = new System.IO.StreamReader( stream, Encode );
                            if( reader != null ) {
                                html = reader.ReadToEnd();
                                reader.Close();
                            }
                            stream.Close();
                        }
                    }
                }
                catch( System.Net.WebException e ) {
                    //e.Status
                    //e.Responce
                    //System.Windows.Forms.MessageBox.Show( "応答取得に失敗しました" );
                }
            }
            return html;
        }

        //test
        public delegate void CallbackFunc( TranslateBase trans );
        public event CallbackFunc AsyncDone;
        System.Threading.ManualResetEvent signal = null;
        public enum ConnectionResult {
            FailureTimeOut,
            FailureResponce,
            FailureUnknown,
            Success,
        }
        ConnectionResult result = ConnectionResult.FailureUnknown;
        public ConnectionResult Result {
            get { return result; }
            protected set { result = value; }
        }


        void process( object o ) {
            TranslateBase trans = o as TranslateBase;
            bool ret = this.signal.WaitOne( 30 * 1000 ); // ms
            if( !ret ) {
                trans.Result = ConnectionResult.FailureTimeOut;
            }
            this.AsyncDone( trans );
        }


        //public bool GetResponseASync( CallbackFunc cb ) {
        public bool GetResponseASync() {
            if( Request != null ) {
                OutputText = null;
                try {
                    IAsyncResult result = Request.BeginGetResponse( new AsyncCallback( ResponseCallback ), this );
                    //callback = cb;
                    signal = new System.Threading.ManualResetEvent( false );
                    signal.Reset();
                    System.Threading.Thread thread = new System.Threading.Thread( new System.Threading.ParameterizedThreadStart( process ) );

                    // activexコンポーネントを含んでいるフォームをMTAで呼び出すと例外が発生する仕様の対応
                    // 事前にフォーム作ってやるようにしたので不要
                    //if( thread.TrySetApartmentState( System.Threading.ApartmentState.STA ) == false ) {
                    //    return false;
                    //}
                    thread.Start( this );
                    return true;
                }
                catch( System.Net.WebException e ) {
                    //System.Windows.Forms.MessageBox.Show( "応答取得開始に失敗しました" );
                    Result = ConnectionResult.FailureResponce;
                }
            }
            return false;
        }
        private static void ResponseCallback( IAsyncResult asyncResult ) {
            TranslateBase trans = asyncResult.AsyncState as TranslateBase;
            // ありえないが、というか駄目だったら素通りしていいの？ end待ちのがのこりそうだが
            if( trans != null ) {
                System.Net.HttpWebRequest req = trans.Request;
                if( req != null ) {
                    // responce done
                    try {
                        System.Net.HttpWebResponse res = req.EndGetResponse( asyncResult ) as System.Net.HttpWebResponse;
                        if( res != null ) {
                            System.IO.Stream stream = res.GetResponseStream();
                            if( stream != null ) {
                                //trans.async = new ASyncState( stream, trans.Encode );
                                trans.async = new ASyncState( stream, Encode ); // 継承先の静的プロパティとなる
                                trans.async.Read( trans );
                            }
                        }
                    }
                    catch ( System.Net.WebException e ) {
                        //System.Windows.Forms.MessageBox.Show( "応答取得に失敗しました" );
                        trans.Result = ConnectionResult.FailureResponce;
                        trans.signal.Set();
                    }
                }
            }
        }

        private class ASyncState {
            public ASyncState( System.IO.Stream s, System.Text.Encoding enc ) {
                stream = s;
                encode = enc;
            }

            public IAsyncResult Read( TranslateBase trans ) {
                IAsyncResult result = stream.BeginRead( buffer, 0, buffer.Length, new AsyncCallback( ReadCallback ), trans );
                return result;
            }

            private static void ReadCallback( IAsyncResult asyncResult ) {
                TranslateBase trans = asyncResult.AsyncState as TranslateBase;
                // ありえないが、というか駄目だったら素通りしていいの？ end待ちのがのこりそうだが
                // TOOD 細かくチェック
                int size = trans.async.stream.EndRead( asyncResult );
                if( size > 0 ) {
                    // next callback
                    trans.async.memory.Write( trans.async.buffer, 0, size );
                    IAsyncResult result = trans.async.stream.BeginRead( trans.async.buffer, 0, trans.async.buffer.Length, new AsyncCallback( ReadCallback ), trans );
                } else {
                    trans.async.stream.Close();
                    byte[] mem = trans.async.memory.ToArray();
                    if( mem != null ) {
                        string html = trans.async.encode.GetString( mem );
                        trans.async.memory.Close();
                        string result = trans.GetTrim( html );
                        trans.OutputText = result;
                    }
                    trans.Result = ConnectionResult.Success;
                    trans.signal.Set();
                }
            }

            System.IO.Stream stream = null;
            byte[] buffer = new byte[ 1024 * 4 ]; // あまり多すぎても使い切らないようだ、スレッド処理が回ってきたタイミングで打ち切られるのかな
            System.IO.MemoryStream memory = new System.IO.MemoryStream(); // byte単位で読まれて文字列に逐次変換はできないので一旦メモリの塊にする
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
        }
        // こちらを使うようにする
        ASyncState async = null;
    }

    public class EijirouTranslate : TranslateBase {
        // newをつけないと単一のstatic TranslateBaseの変数にアクセスしにいく
        public new static System.Text.Encoding Encode {
            get { return encode; }
            private set { encode = value; }
        }

        public EijirouTranslate( string text ) {
            Encode = System.Text.Encoding.UTF8;
            Create( text );
        }

        static public string GetUrl( string text ) {
            string data = System.Web.HttpUtility.UrlEncode( text, Encode );
            string url = "http://eow.alc.co.jp/" + data + "/UTF-8/";
            return url;
        }

        protected override bool Create( string text ) {
            URL = GetUrl( text );
            Request = CreateRequest( URL, UserAgent );
            if( Request != null ) {
                return true;
            }
            return false;
        }

        protected override string GetTrim( string html ) {
            if( html == null ) {
                return null;
            }

            string startStr = "<!-- ▼ 検索補助 ▼ -->";
            string endStr = "<!-- ▲ 検索結果本体 ▲ -->";
            int trimStartIdx = html.IndexOf( startStr ) + startStr.Length;
            int trimEndIdx = html.IndexOf( endStr, trimStartIdx );
            string result = html.Substring( trimStartIdx, trimEndIdx - trimStartIdx );
            // a tag javascript
            result = result.Replace( "<a href='javascript:goGradable(\"", "<a href=\"" );
            result = result.Replace( "\")'>", "\">" );

            // expand idiom
            result = result.Replace( "... <a href=\"javascript:changeIdiomDisplay('idiom1', 'idiom2')\">【もっとイディオムを見る】</a>", "/ " );

            // idiom2 span
            result = result.Replace( "style=\"display:none;\"", "" );

            // target
            result = result.Replace( "target=\"eow_idm\"", "" );

            // ダブルクリックでそれを検索の除去
            result = result.Replace( "ondblclick=\"seow()\"", "" );
            //result = result.Replace( "<br>", "\r\n" ); // 向こうが使っているやつ
            return result;

        }

    }

    public class GoogleTranslate : TranslateBase {
        // newをつけないと単一のstatic TranslateBaseの変数にアクセスしにいく
        public new static System.Text.Encoding Encode {
            get { return encode; }
            private set { encode = value; }
        }
        public GoogleTranslate( string text ) {
            Encode = System.Text.Encoding.UTF8;
            Create( text );
        }
        
        protected override bool Create( string text ) {
            byte[] data = CreatePostData( text );
            URL = "http://translate.google.com/";
            Request = CreateRequest( URL, UserAgent );
            if( Request != null ) {
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded"; // header
                Request.ContentLength = data.Length;

                // using でdispose呼んで大丈夫とも思えん
                // 例外処理が必要
                System.IO.Stream stream = Request.GetRequestStream();
                if( stream != null ) {
                    stream.Write( data, 0, data.Length );
                    stream.Close();
                    InputText = text;
                    return true;
                }
            }
            return false;
        }

        protected byte[] CreatePostData( string text ) {
            // EtoJ
            string param = "";
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht[ "ie" ] = "UTF-8"; // Encodeに準拠しないとだめ
            ht[ "hl" ] = "ja";
            ht[ "oe" ] = "UTF-8";
            ht[ "text" ] = System.Web.HttpUtility.UrlEncode( text, Encode );
            ht[ "langpair" ] = "en|ja";
            ht[ "gtrans" ] = "";
            foreach( string k in ht.Keys ) {
                param += String.Format( "{0}={1}&", k, ht[ k ] );
            }
            byte[] data = Encoding.ASCII.GetBytes( param );
            return data;
        }

        protected override string GetTrim( string html ) {
            // 改行されている場合は複数回繰り返さないと駄目なコードだな。もしくは改行を取ってから渡すか
            string startStr = "onmouseover=\"this.style.backgroundColor='#ebeff9'\" onmouseout=\"this.style.backgroundColor='#fff'\">";
            string endStr = "</span>";
            int trimStartIdx = html.IndexOf( startStr ) + startStr.Length;
            int trimEndIdx = html.IndexOf( endStr, trimStartIdx );
            string result = html.Substring( trimStartIdx, trimEndIdx - trimStartIdx );
            result = result.Replace( "<br>", "\r\n" ); // 向こうが使っているやつ
            return result;
        }

    }

}
