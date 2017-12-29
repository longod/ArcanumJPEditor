// (c) hikami, aka longod

#include <windows.h>
#include <shlwapi.h>

#include <iostream>
#include <fstream>
#include <cassert>
#include <string>
#include <map>

#pragma comment(lib, "shlwapi.lib")


#ifdef _DEBUG
#define NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#else
#define NEW new
#endif

#define SAFE_DELETE(p)  { if(p) { delete (p);     (p)=NULL; } }
#define SAFE_DELETE_ARRAY(p) { if (p) { delete[] (p);   (p)=NULL; } }

namespace util {

    bool isLeadByte( unsigned char c ) {
        return (::IsDBCSLeadByte(c) == 0) ? false : true; // }��0x7d�ł�true���A���Ă��邼�Aunsigned�łȂ��ƕ������c
        //return (((c >= 0x81) && (c <= 0x9f)) || ((c >= 0xe0) && (c <= 0xfc))); // ���O�����
    }

    bool isIgnoreLetter( char c ) {
        char atmark = c ^ 0x40;
        char curl = c ^ 0x7d;
        if ( atmark == 0 || curl == 0 ) {
            return true;
        }
        return false;
    }

    char* getNextChar( char* ptr ) {
        return ::CharNext( ptr );
    }

    // �t���p�X�̂�
    void splitPath( const char *path, char* dir, int dir_size, char* name, int name_size ) {
        char *ptr = const_cast<char*>(path);
        char* split = NULL;

        while(*ptr != '\0') {
            // 2�o�C�g�����̐擪�̓X�L�b�v
            if( isLeadByte(*ptr) == false ) {
                //[\],[/],[:]���������猻�ݒn�̃A�h���X��ۑ�
                if( (*ptr == '\\') || (*ptr == '/') || (*ptr == ':') ) {
                    split = ptr;
                }
            }
            //���̕�����
            ptr = getNextChar(ptr);
        }

        int plen = static_cast<int>(::strlen(path));
        if ( split ) {
            int nlen = static_cast<int>(::strlen(split));
            int dlen = plen - nlen;

            if ( nlen + 1 < name_size && name ) {
                ::memcpy_s( name, name_size, split, nlen );
                name[nlen] = '\0';            
            }

            if ( dlen >= 0 && dlen + 1 < dir_size && dir ) {
                ::memcpy_s( dir, dir_size, path, dlen ); // or strcpy�I�[\0���ᖳ���̂ł���
                dir[dlen] = '\0';            
            }
        } else {
            // ������Ȃ������̂őS���t�@�C�����H
            if ( plen + 1 < name_size && name ) {
                ::memcpy_s( name, name_size, path, plen );
                name[plen] = '\0';            
            }
            if ( dir_size > 0 && dir ) {
                dir[0] = '\0';
            }

        }

    }

    // �ċN�f�B���N�g���쐬
    void createDirectory( const char* path ) {
        const int max_path = MAX_PATH + 1;
        char output_dir[ max_path ];
        ::memset( output_dir, 0x0, sizeof(output_dir)/sizeof(char) );
        util::splitPath( path, output_dir, max_path, NULL, 0 );
        size_t len = ::strlen( output_dir );
        // ���΃p�X�̋󂩃t���p�X�̃h���C�u���^�[���l��
        if ( len > 0 && output_dir[len-1] != ':') {
            createDirectory( output_dir );
            ::CreateDirectory( output_dir, NULL );
        }

    }

}

namespace io {
    bool read( const char *file, char *&buff, size_t &size ) {
        assert(file);
        assert(buff == NULL);

        std::ifstream infile( file, std::ifstream::binary );
        if ( infile.fail() ) {
            std::cout << "[ERROR] Can't read: " << file << std::endl;
            return false;
        }

        infile.seekg( 0, std::ifstream::end );
        size_t s = static_cast<size_t>(infile.tellg());
        infile.seekg( 0, std::ifstream::beg );
        buff = NEW char[s];
        infile.read( buff, static_cast<std::streamsize>(s) );
        infile.close();

        size = s;

        return true;
    }

    bool write( const char *file, const char *buff, size_t size ) {
        assert(file);
        assert(buff);

        std::ofstream outfile( file, std::ostream::binary );
        if ( outfile.fail() ) {
            std::cout << "[ERROR] Can't open: " << file << std::endl;
            return false;
        }
        outfile.write( buff, static_cast<std::streamsize>(size) );
        outfile.close();
        return true;
    }
}



namespace replacer {
    typedef unsigned short Key;
    typedef unsigned short Value;
    typedef std::map< Key, Value > IgnoreMap;
    typedef std::pair< Key, Value > IgnorePair;

    bool createIgnoreMap( const char* path, IgnoreMap& ignore ) {
        unsigned char *buff = NULL;
        size_t size = 0;

        bool ret = false;
        ret = io::read( path, reinterpret_cast<char*&>(buff), size );
        if ( !ret ) {
            std::cout << "[ERROR] Can't read: " << path << std::endl;
            return ret;
        }

        //bool isDBC = false;
        bool isKey = true;
        replacer::Key key = 0;
        replacer::Key value = 0;
        for ( size_t i = 0; i < size; ++i ) {
            unsigned char letter = buff[i];
            switch ( letter )
            {
            case '=':
                isKey = false;
                break;
            case '\r':
                break;
            case '\n':
                if ( key != 0 && value != 0 ) {
                    ignore.insert( IgnorePair(key, value) );
                }
                isKey = true;
                key = 0;
                value = 0;
                break;
            default:
                // 0xf181�Ƃ��ŊO���̉��ʃo�C�g���_�u���o�C�g�Ɍ�F�����̂Œ���
                // �t�H���g�̔z�u�����炷�̂��肾���A���蓖�Ă鐔������Ȃ����H
                bool isDBC = util::isLeadByte( letter );
                if ( isDBC ) {
                    if ( i < (size-1) ) {
                        unsigned char lower = buff[i+1];
                        // lower��\n�Ƃ��ُ�ȃf�[�^�̏ꍇ�͂ǂ�����H�`�F�b�N�������邩�H
                        // ���Ƃ����} @���Ƃ��������̂Ōy���`�F�b�N
                        switch ( lower )
                        {
                        case '}':
                        case '@':
                            if ( !isKey ) {
                                std::cout << "[WARNING] Including 0x40 or 0x7D at list" << std::endl;
                            }
                            break;
                        default:
                            break;
                        }
                        if ( isKey ) {
                            key = lower;
                            key |= letter << 8;
                        } else {
                            value = lower;
                            value |= letter << 8;
                        }
                    }
                    ++i; // �_�u���o�C�g�Ȃ̂�next
                }
      
                // ���p���A�������ꍇ�͍l�����Ă��Ȃ��Ȃ�
                break;
            }
        }

        SAFE_DELETE_ARRAY( buff );

        return ret;
    }


    bool replace( const char* input, const char* output, IgnoreMap& ignore ) {
        unsigned char *buff = NULL;
        size_t size = 0;

        bool ret = false;
        ret = io::read( input, reinterpret_cast<char*&>(buff), size );
        if ( !ret ) {
            std::cout << "[ERROR] Can't read: " << input << std::endl;
            return ret;
        }

        util::createDirectory(output);

        for ( size_t i = 0; i < size; ++i ) {
            char letter = buff[i];
            switch ( letter )
            {
            case '\r':
                break;
            case '\n':
                break;
            default:
                bool isDBC = util::isLeadByte( letter ); // 0x8100~0xFF00�܂œ��邩�ȁH
                // �ۑ�����̂͏�ʃr�b�g�����ł悭�ˁH�Ƃ���������value�͗��������
                if ( isDBC ) {
                    if ( i < (size-1) ) {
                        unsigned char lower = buff[i+1];
                        bool isIgnore = util::isIgnoreLetter(lower);
                        //isIgnore = true; // debug
                        if ( isIgnore ){
                            Key key = lower;
                            key |= letter << 8;
                            IgnoreMap::iterator ite = ignore.find( key );
                            if ( ite != ignore.end() ){
                                Key k = ite->first;
                                Value v = ite->second;
                                unsigned char hi = (v & 0xFF00) >> 8;
                                unsigned char low = v & 0x00FF;
                                buff[i] = hi;
                                buff[i + 1] = low;
                                // 2byte -> 1byte�Ƃ����l����Ɠ����o�b�t�@������������ăR�s�[���Ȃ��炩�Ȃ�
                            } else {
                                // ���X�g�ɖ������_������
                                std::cout << "[WARNING] Detected Damemoji. but not found on list : " << lower << std::endl;
                            }
                        }
                    }
                    ++i; // �_�u���o�C�g�Ȃ̂�next
                } else {
                    // �g�[�N������������Ă��Ȃ����`�F�b�N���悤
                    // 1:1�őΉ����Ƃ�Ă��邩
                    switch ( letter )
                    {
                    case '{':
                        break;
                    case '}':
                        break;
                    case '@':
                        break;
                    default:
                        break;
                    }
                }
                break;
            }
        }

        ret = io::write( output, reinterpret_cast<char*>(buff), size );

        SAFE_DELETE_ARRAY( buff );

        return ret;
    }
}

int main( int argc, char* argv[] ) {
#ifdef _DEBUG
    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
#endif
    // ���{�ꂵ���z�肵�Ă��Ȃ�
    ::setlocale(LC_ALL, "japanese" );
    std::locale::global(std::locale("japanese"));

    if ( argc != 3 ) {
        std::cout << "Arcanum Damemoji Replacer" << std::endl;
        std::cout << "USAGE: adr <inputpath> <outputpath>" << std::endl;
        std::cout << "EX: adr dlg\\hoge.dlg conv\\dlg\\hoge.dlg" << std::endl;
        return -1;
    }

    const char* input_path = argv[1];
    char* output_path  = argv[2]; // �Ƃ肠�����͊m���Ƀf�B���N�g�����w�肳��Ȃ��Ɖ��肵�Ă�����

    const int max_path = MAX_PATH + 1;

    char module_path[ max_path ];
    ::memset( module_path, 0x0, sizeof(module_path)/sizeof(char) );
    ::GetModuleFileName( NULL, module_path, max_path );

    char module_dir[ max_path ];
    ::memset( module_dir, 0x0, sizeof(module_dir)/sizeof(char) );
    util::splitPath( module_path, module_dir, max_path, NULL, 0 );

    // open ignore list
    char ignore_path[ max_path ];
    ::memset( ignore_path, 0x0, sizeof(ignore_path)/sizeof(char) );
    ::strcpy_s( ignore_path, max_path, module_dir );
    ::strcat_s( ignore_path, max_path, "\\damemoji.lst" );

    replacer::IgnoreMap ignore;
    bool ret = replacer::createIgnoreMap( ignore_path, ignore );
    if ( !ret ) {
        return -2;
    }

    char input_fullpath[ max_path ];
    ::memset( input_fullpath, 0x0, sizeof(input_fullpath)/sizeof(char) );
    if ( _fullpath( input_fullpath, input_path, max_path ) == NULL ) {
        std::cout << "[ERROR] Can't find: " << input_path << std::endl;
        return -3;
    }

    char output_fullpath[ max_path ];
    ::memset( output_fullpath, 0x0, sizeof(output_fullpath)/sizeof(char) );
    if ( _fullpath( output_fullpath, output_path, max_path ) == NULL ) {
        std::cout << "[ERROR] Can't find: " << output_path << std::endl;
        return -4;
    }

    std::cout << "Replace: "  << input_fullpath << " -> " << output_fullpath << std::endl;
    bool rep = replacer::replace( input_fullpath, output_fullpath, ignore );
    if ( !rep ) {
        return -5;
    }
    std::cout << "Succeed." << std::endl;

    return 0;
}

