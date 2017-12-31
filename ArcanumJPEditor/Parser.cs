// (c) hikami, aka longod
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcanumJPEditor {
    public class Format {
        public Format() {
        }
        public class Pair {
            public Pair() {
            }
            public string Key;
            public string Value;
        }
        public List<Pair> TestFormat = new List<Pair>();
        public List<Pair> ResultFormat = new List<Pair>();
        public List<Pair> DialogFormat = new List<Pair>();
    }
    public class KeyArguments {
        public string Key;
        public List<string> Args = new List<string>();
    }
    public class Parser {
        static Parser instance = null;

        System.Collections.Hashtable Mes = new System.Collections.Hashtable();
        System.Collections.Hashtable funcsTest = new System.Collections.Hashtable();
        System.Collections.Hashtable funcsResult = new System.Collections.Hashtable();
        System.Collections.Hashtable funcsDialog = new System.Collections.Hashtable();
        System.Collections.Hashtable textTest = new System.Collections.Hashtable();
        System.Collections.Hashtable textResult = new System.Collections.Hashtable();
        System.Collections.Hashtable textDialog = new System.Collections.Hashtable();

        // ����Ƀe���v���[�g�g���ă^�C�v�w�肵�Ă����ƁAtest��result�֐��g���Ƃ���������^�p���h���邩�H
        delegate string GetValue( string name, List<string> args, File.Node sender );

        private Parser() {
        }
        private Parser( string fullpath ) {
            //���\�[�X���[�h

            // 1��function�ł�肽�����Bkey���ƂɕK�v�ȃ��\�[�X���}�b�v�ɂ��ČĂׂ�悤�ɂ��Ă�

            // �e�[�u���쐬
            funcsTest.Add( "$$", new GetValue( test ) );
            funcsTest.Add( "al", new GetValue( test ) );
            funcsTest.Add( "ar", new GetValue( testArea ) );
            funcsTest.Add( "ch", new GetValue( test ) );
            funcsTest.Add( "fo", new GetValue( test ) );
            funcsTest.Add( "gf", new GetValue( testGlobalFlags ) );
            funcsTest.Add( "gv", new GetValue( testGlobalVars ) );
            funcsTest.Add( "ha", new GetValue( test ) );
            funcsTest.Add( "ia", new GetValue( testArea ) );
            funcsTest.Add( "in", new GetValue( testNameIndex ) );
            funcsTest.Add( "lc", new GetValue( test ) );
            funcsTest.Add( "le", new GetValue( test ) );
            funcsTest.Add( "lf", new GetValue( test ) );
            funcsTest.Add( "ma", new GetValue( test ) );
            funcsTest.Add( "me", new GetValue( test ) );
            funcsTest.Add( "na", new GetValue( test ) );
            funcsTest.Add( "ni", new GetValue( testNameIndex ) );
            funcsTest.Add( "pa", new GetValue( testNameIndex ) );
            funcsTest.Add( "pe", new GetValue( test ) );
            funcsTest.Add( "pf", new GetValue( test ) );
            funcsTest.Add( "ps", new GetValue( test ) );
            funcsTest.Add( "pv", new GetValue( test ) );
            funcsTest.Add( "qa", new GetValue( testQuest ) );
            funcsTest.Add( "qb", new GetValue( testQuest ) );
            funcsTest.Add( "qu", new GetValue( testQuest ) );
            funcsTest.Add( "ra", new GetValue( testRace ) );
            funcsTest.Add( "re", new GetValue( test ) );
            funcsTest.Add( "rp", new GetValue( testReputation ) );
            funcsTest.Add( "rq", new GetValue( testRumor ) );
            funcsTest.Add( "ru", new GetValue( testRumor ) );
            funcsTest.Add( "sc", new GetValue( testSpellCollege ) );
            funcsTest.Add( "sk", new GetValue( testSkill ) );
            funcsTest.Add( "ss", new GetValue( testStoryState ) );
            funcsTest.Add( "ta", new GetValue( test ) );
            funcsTest.Add( "tr", new GetValue( testTraining ) );
            funcsTest.Add( "wa", new GetValue( test ) );
            funcsTest.Add( "wt", new GetValue( test ) );

            funcsResult.Add( "$$", new GetValue( result ) );
            funcsResult.Add( "al", new GetValue( result ) );
            funcsResult.Add( "ce", new GetValue( result ) );
            funcsResult.Add( "co", new GetValue( result ) );
            funcsResult.Add( "et", new GetValue( resultExpertTraining ) );
            funcsResult.Add( "fl", new GetValue( resultFloatLine ) );
            funcsResult.Add( "fp", new GetValue( result ) );
            funcsResult.Add( "gf", new GetValue( resultGlobalFlags ) );
            funcsResult.Add( "gv", new GetValue( resultGlobalVars ) );
            funcsResult.Add( "ii", new GetValue( result ) );
            funcsResult.Add( "in", new GetValue( resultNameIndex ) );
            funcsResult.Add( "jo", new GetValue( resultJoin ) );
            funcsResult.Add( "lc", new GetValue( result ) );
            funcsResult.Add( "lf", new GetValue( result ) );
            funcsResult.Add( "lv", new GetValue( result ) );
            funcsResult.Add( "ma", new GetValue( result ) );
            funcsResult.Add( "mm", new GetValue( resultMarkMap ) );
            funcsResult.Add( "nk", new GetValue( result ) );
            funcsResult.Add( "np", new GetValue( resultNewspaper ) );
            funcsResult.Add( "or", new GetValue( result ) );
            funcsResult.Add( "pf", new GetValue( result ) );
            funcsResult.Add( "pv", new GetValue( result ) );
            funcsResult.Add( "qu", new GetValue( resultQuest ) );
            funcsResult.Add( "re", new GetValue( result ) );
            funcsResult.Add( "ri", new GetValue( result ) );
            funcsResult.Add( "rp", new GetValue( resultReputation ) );
            funcsResult.Add( "rq", new GetValue( resultRumor ) );
            funcsResult.Add( "ru", new GetValue( resultRumor ) );
            funcsResult.Add( "sc", new GetValue( result ) );
            funcsResult.Add( "so", new GetValue( result ) );
            funcsResult.Add( "ss", new GetValue( resultStoryState ) );
            funcsResult.Add( "su", new GetValue( result ) );
            funcsResult.Add( "tr", new GetValue( resultTraining ) );
            funcsResult.Add( "uw", new GetValue( resultJoin ) );
            funcsResult.Add( "wa", new GetValue( result ) );
            funcsResult.Add( "xp", new GetValue( resultXP ) );

            // ���ʌ���͌����ɂ�GeneratedDialog�ł͂Ȃ����A��b���C���ɑ΂��Ă̏����Ƃ������Ƃ�
            funcsDialog.Add( "0", new GetValue( dialog ) );
            funcsDialog.Add( "1", new GetValue( dialog ) );
            funcsDialog.Add( "A", new GetValue( dialog ) );
            funcsDialog.Add( "B", new GetValue( dialog ) );
            funcsDialog.Add( "C", new GetValue( dialog ) );
            funcsDialog.Add( "D", new GetValue( dialogDirections ) );
            funcsDialog.Add( "E", new GetValue( dialog ) );
            funcsDialog.Add( "F", new GetValue( dialog ) );
            funcsDialog.Add( "G", new GetValue( dialogGreeting ) );
            funcsDialog.Add( "H", new GetValue( dialog ) );
            funcsDialog.Add( "I", new GetValue( dialog ) );
            funcsDialog.Add( "J", new GetValue( dialog ) );
            funcsDialog.Add( "K", new GetValue( dialog ) );
            funcsDialog.Add( "L", new GetValue( dialog ) );
            funcsDialog.Add( "M", new GetValue( dialogMoney ) );
            funcsDialog.Add( "N", new GetValue( dialog ) );
            funcsDialog.Add( "O", new GetValue( dialog ) );
            funcsDialog.Add( "P", new GetValue( dialog ) );
            funcsDialog.Add( "Q", new GetValue( dialog ) );
            funcsDialog.Add( "R", new GetValue( dialogRumors ) );
            funcsDialog.Add( "S", new GetValue( dialog ) );
            funcsDialog.Add( "T", new GetValue( dialogTraining ) );
            funcsDialog.Add( "U", new GetValue( dialogUseskill ) );
            funcsDialog.Add( "V", new GetValue( dialog ) );
            funcsDialog.Add( "W", new GetValue( dialog ) );
            funcsDialog.Add( "X", new GetValue( dialogXMarkstheSpot ) );
            funcsDialog.Add( "Y", new GetValue( dialog ) );
            funcsDialog.Add( "Z", new GetValue( dialogZapspell ) );
            //textDialog.Add( "0", "text:" + "female" );
            //textDialog.Add( "1", "text:" + "male" );
            //for( char word = 'A'; word <= 'Z'; ++word ) {
            //    string key = new string( word, 1 );
            //    funcsDialog.Add( key, new GetValue( result ) );
            //    //textDialog.Add( key, "text:" + key );
            //}
#if false
            textTest.Add( "$$", @"PC �ƒ��Ԃ̏������� #1 gold ?1{��葽��,,��菭�Ȃ�}�B" );
            textTest.Add( "al", @"PC �� Alignment �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "ar", @"PC �̓G���A %1 ��?1{�������Ă���,�������Ă���,�������Ă��Ȃ�}�B" );
            textTest.Add( "ch", @"PC �� Charisma �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "fo", @"NPC �� PC ��!1{���Ԃł���,���Ԃł͂Ȃ�}�B" );
            textTest.Add( "gf", @"Global Flag #1 ��!2{�����Ă���,�����Ă��Ȃ�}�B" );
            textTest.Add( "gv", @"Global Variable #1 �� #2 �ł���B" );
            textTest.Add( "ha", @"PC �� Haggle �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "ia", @"PC ���G���A %1 ��?1{����,����,���Ȃ�}�B" );
            textTest.Add( "in", @"?1{PC �܂��͒���,PC �܂��͒���,NPC}���A�C�e�� %1 �������Ă���B" );
            textTest.Add( "lc", @"Local Counter #1 �� #2 �ł���B" );
            textTest.Add( "le", @"PC �� Level �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "lf", @"Local Flag #1 ��!2{�����Ă���,�����Ă��Ȃ�}�B" );
            textTest.Add( "ma", @"PC �� Magical Aptitude �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "me", @"�ȑO�� NPC �� PC �� !1{����Ă���,����Ă��Ȃ�}�B" );
            textTest.Add( "na", @"PC �� Alignment �� -#1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "ni", @"?1{PC �܂��͒���,PC �܂��͒���,NPC} ���A�C�e�� %1 �������Ă��Ȃ��B" );
            textTest.Add( "pa", @"PC �̃p�[�e�B�ɒ��� %1 �� ?1{������Ă���,������Ă���,������Ă��Ȃ�}�B" );
            textTest.Add( "pe", @"PC �� Perception �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "pf", @"PC Flag #1 �� !2{�����Ă���,�����Ă��Ȃ�}�B" );
            textTest.Add( "ps", @"PC �� Persuasion �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "pv", @"PC Variable #1 �� #2 �ł���B" );
            textTest.Add( "qa", @"�N�G�X�g #1 �̏�Ԃ� %2 �ȏ�i�W���Ă���B" );
            textTest.Add( "qb", @"�N�G�X�g #1 �̏�Ԃ� %2 ���i�W���Ă��Ȃ��B" );
            textTest.Add( "qu", @"�N�G�X�g #1 �̏�Ԃ� %2 �ł���B" );
            textTest.Add( "ra", @"PC �� Race �� %1 ?1{�ł���,�ł���,�ł͂Ȃ�}�B" );
            textTest.Add( "re", @"NPC �� Reaction �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "rp", @"PC �� Reputation #1 ��?1{���Ă���,���Ă���,���Ă��Ȃ�}�B" );
            textTest.Add( "rq", @"Rumor #1 ��?1{���܂��Ă���,���܂��Ă���,���܂��Ă��Ȃ�}�B" ); // ���Ή�
            textTest.Add( "ru", @"PC �̃��O�� Rumor #1 �� ?1{�L�^����Ă���,�L�^����Ă���,�L�^����Ă��Ȃ�}�B" ); // ���Ή�
            textTest.Add( "sc", @"PC �� College %1 �� Spell �� #2 ?2{�ȏ�,�ȏ�,�ȉ�}�m���Ă���B" );
            textTest.Add( "sk", @"PC �� Skill %1 �� #2 ?2{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "ss", @"���݂� Story State �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "ta", @"PC �� Tech Aptitude �� #1 ?1{�ȏ�,�ȏ�,�ȉ�}�B" );
            textTest.Add( "tr", @"PC �� Skill %1 �� Rank �� %2 ?2{�ȏ�,,�ȉ�}�B" );
            textTest.Add( "wa", @"NPC ��!1{�ҋ@���Ă���,�ҋ@���Ă��Ȃ�}�B" );
            textTest.Add( "wt", @"NPC �̑ҋ@���邱�Ƃ��ł��鎞�Ԃ�!1{�߂��Ă���,�߂��Ă��Ȃ�}�B" );

            textResult.Add( "$$", @"?1{PC ��,PC ��,PC �ƒ��Ԃ���} #1 gold ?1{������,������,���炷}�B" );
            textResult.Add( "al", @"PC �� Alignment �� #1 ?1{������,���Z�b�g����,���炷,�ȏ�ɂ��Ȃ�,�ȉ��ɂ��Ȃ�}�B" );
            textResult.Add( "ce", @"�_�C�A���O����ANPC �� Character Editor �� passive mode �ŊJ���B" );
            textResult.Add( "co", @"�_�C�A���O����ANPC �Ƃ̐퓬���J�n����B" );
            textResult.Add( "et", @"PC �� Skill %1 �� Expert Training ���J�n�ł���̂Ȃ�΁ALine #2 �ւƑ����B�����łȂ��̂Ȃ�� Response Line �ɑ����B" );
            textResult.Add( "fl", @"�_�C�A���O����A Float Line #1 �� NPC ����ɏo���B" );
            textResult.Add( "fp", @"PC �� Fate Point �� 1 �^����B" );
            textResult.Add( "gf", @"Global Flag #1 �� !2{���Ă�,�~�낷}�B" );
            textResult.Add( "gv", @"Global Variable #1 �� #2 �ɂ���B" );
            textResult.Add( "ii", @"Inventory UI �� identify mode �ŊJ�n����B" );
            textResult.Add( "in", @"?1{PC �܂��͒��Ԃ��� NPC ��,PC �܂��͒��Ԃ��� NPC ��,NPC ���� PC ��}�A�C�e�� %1 ��n���B" );
            textResult.Add( "jo", @"Charisma �ɂ��l�������`�F�b�N��!1{����,���Ȃ���} NPC ���p�[�e�B�ɉ�����B�o�����Ȃ�� Line #2 �ւƑ����B�o���Ȃ������Ȃ��Response Line �ɑ����B" );
            textResult.Add( "lc", @"Local Counter #1 �� #2 �ɂ���B" );
            textResult.Add( "lf", @"Local Flag #1 �� !2{���Ă�,�~�낷}�B" );
            textResult.Add( "lv", @"NPC ���p�[�e�B���痣�E����B" );
            textResult.Add( "ma", @"PC �� Magical Aptitude �� #1 ?1{������,���Z�b�g����,���炷}�B" );
            textResult.Add( "mm", @"PC �̒n�}�ɃG���A %1 �̈������B" );
            textResult.Add( "nk", @"NPC �����Ȃ���B" );
            textResult.Add( "np", @"�D��x��!2{�Ȃ�,����}�V�� #1 ��^����B" ); //
            textResult.Add( "or", @"NPC �� origin �� #1 �ɂ���B" );
            textResult.Add( "pf", @"PC Flag #1 �� !2{���Ă�,�~�낷}�B" );
            textResult.Add( "pv", @"PC Variable #1 �� #2 �ɃZ�b�g����B" );
            textResult.Add( "qu", @"�N�G�X�g #1 �̏�Ԃ� %2 �ɂ���B" );
            textResult.Add( "re", @"NPC �� Reaction �� #1 ?1{������,���Z�b�g,���炷,�ȏ�ɂ��Ȃ�,�ȉ��ɂ��Ȃ�}�B" );
            textResult.Add( "ri", @"Inventory UI �� repair mode �ŊJ���B" );
            textResult.Add( "rp", @"PC �� Reputation #1 �� ?1{�^����,�^����,��菜��}�B" );
            textResult.Add( "rq", @"Rumor #1 ����߂�B" ); // ���Ή�
            textResult.Add( "ru", @"Rumor #1 �� PC�̃��O�ɒǉ�����B" ); // ���Ή�
            textResult.Add( "sc", @"NPC �𖧏W������B" );
            textResult.Add( "so", @"NPC ���U�J������B" );
            textResult.Add( "ss", @"���݂� Story State �� #1 ��菬�����ꍇ #1 �ɂ���B" );
            textResult.Add( "su", @"Schematic UI ���J���B" );
            textResult.Add( "ta", @"PC �� Tech Aptitude �� #1 ?1{������,���Z�b�g����,���炷}�B" );
            textResult.Add( "tr", @"Skill %1 �̃����N�� %2 �ɂ���B" );
            textResult.Add( "uw", @"Charisma �ɂ��l�������`�F�b�N��!1{����,���Ȃ���} �ANPC �̑ҋ@��Ԃ������ăp�[�e�B�ɍĉ���������B�o�����Ȃ�� Line #2 �ւƑ����B�o���Ȃ������Ȃ�� Response Line �ɑ����B" );
            textResult.Add( "wa", @"NPC ��ҋ@������B" );
            textResult.Add( "xp", @"PC �� �N�G�X�g���x�� #1 �����̌o���l���l������B" );
#endif

#if false // dump xml
            Format format = new Format();
            {
                Format.Pair[] pairs = new Format.Pair[ textTest.Count ];
                int count = 0;
                foreach ( string key in textTest.Keys ) {
                    pairs[ count ] = new Format.Pair();
                    pairs[ count ].Key = key;
                    ++count;
                }
                count = 0;
                foreach ( string val in textTest.Values ) {
                    //pairs[ count ] = new Format.Pair();
                    pairs[ count ].Value = val;
                    ++count;
                }
                format.TestFormat.AddRange( pairs );
            }
            {
                Format.Pair[] pairs = new Format.Pair[ textResult.Count ];
                int count = 0;
                foreach ( string key in textResult.Keys ) {
                    pairs[ count ] = new Format.Pair();
                    pairs[ count ].Key = key;
                    ++count;
                }
                count = 0;
                foreach ( string val in textResult.Values ) {
                    //pairs[ count ] = new Format.Pair();
                    pairs[ count ].Value = val;
                    ++count;
                }
                format.ResultFormat.AddRange( pairs );
            }
            {
                Format.Pair[] pairs = new Format.Pair[ textGeneratedDialog.Count ];
                int count = 0;
                foreach ( string key in textGeneratedDialog.Keys ) {
                    pairs[ count ] = new Format.Pair();
                    pairs[ count ].Key = key;
                    ++count;
                }
                count = 0;
                foreach ( string val in textGeneratedDialog.Values ) {
                    //pairs[ count ] = new Format.Pair();
                    pairs[ count ].Value = val;
                    ++count;
                }
                format.DialogFormat.AddRange( pairs );
            }
            xml.Xml.Write<Format>( Path.Xml.FormatFile, format );
#endif
            Format format = xml.Xml.Read<Format>( Path.Xml.FormatFile );
            foreach ( Format.Pair pair in format.TestFormat ) {
                textTest.Add( pair.Key, pair.Value );
            }
            foreach ( Format.Pair pair in format.ResultFormat ) {
                textResult.Add( pair.Key, pair.Value );
            }
            foreach ( Format.Pair pair in format.DialogFormat ) {
                textDialog.Add( pair.Key, pair.Value );
            }
            //xml.Xml.Write<Format>( Path.Xml.FormatFile, format );
#if false
            textTest.Add( "$$", @"PC and followers have ?1{at least,,no more than} #1 gold" );
            textTest.Add( "al", @"PC's alignment ?1{>,,<}= #1" );
            textTest.Add( "ar", @"PC is ?1{,,NOT} aware of area #1" );
            textTest.Add( "ch", @"PC's Charisma is ?1{>,,<}= #1" );
            textTest.Add( "fo", @"NPC is !1{,not} a follower of PC" );
            textTest.Add( "gf", @"global flag #1 is !2{set,NOT set}" );
            textTest.Add( "gv", @"global variable #1 is equal to value #2" );
            textTest.Add( "ha", @"PC's Haggle ?1{>,,<}= #1" );
            textTest.Add( "ia", @"PC is ?1{,,NOT} in area #1" );
            textTest.Add( "in", @"?1{PC or any follower,PC or any follower,NPC} has item with name index #1" );
            textTest.Add( "lc", @"local counter #1 is equal to #2" );
            textTest.Add( "le", @"if PC's level is ?1{>,,<}= #1" );
            textTest.Add( "lf", @"local flag #1 is !2{set,NOT set}" );
            textTest.Add( "ma", @"PC's Magical Aptitude is ?1{>,,<}= #1" );
            textTest.Add( "me", @"NPC has !1{,not} met PC before" );
            textTest.Add( "na", @"PC's alignment is ?1{>,,<}= -#1" );
            textTest.Add( "ni", @"?1{PC and followers do,PC and followers do,NPC does} NOT have item with name index #1" );
            textTest.Add( "pa", @"follower with name index #1 is ?1{,,NOT} in the PC's party" );
            textTest.Add( "pe", @"PC's Perception is ?1{>,,<}= #1" );
            textTest.Add( "pf", @"PC flag #1 is !2{set,NOT set}" );
            textTest.Add( "ps", @"PC's Persuasion is ?1{>,,<}= #1" );
            textTest.Add( "pv", @"PC variable #1 is equal to value #2" );
            textTest.Add( "qa", @"quest #1 is in a state >= #2" );
            textTest.Add( "qb", @"quest #1 is in a state <= #2" );
            textTest.Add( "qu", @"quest #1 is in state #2 (&Q2)" );
            textTest.Add( "ra", @"PC's race is ?1{,,not} #1 (&R1)" );
            textTest.Add( "re", @"NPC's reaction to PC is ?1{>,,<}= #1" );
            textTest.Add( "rp", @"PC ?1{has,,does NOT have} the reputation #1" );
            textTest.Add( "rq", @"rumor #1 is ?1{,,NOT} quelled" );
            textTest.Add( "ru", @"PC ?1{has,,does NOT have} rumor #1 in log" );
            textTest.Add( "sc", @"PC knows ?2{at least,,no more than} #2 spells in college #1 (&C1)" );
            textTest.Add( "sk", @"PC's training in skill #1 (&S1) is ?2{>,,<}= #2 " );
            textTest.Add( "ss", @"the current story state is ?1{>,,<}= #1" );
            textTest.Add( "ta", @"PC's Tech Aptitude is ?1{>,,<}= #1" );
            textTest.Add( "tr", @"PC's rank in skill #1 (&S1) is ?2{>,,<}= #2 (&D2)" );
            textTest.Add( "wa", @"NPC is !1{,not} currently waiting for his leader to pick him up" );
            textTest.Add( "wt", @"NPC !1{,has not} waited for his leader and the time expired" );

            textResult.Add( "$$", @"?1{add,add,remove} #1 gold ?1{to PC,to PC,from PC and followers}" );
            textResult.Add( "al", @"?1{set PC's alignment to,set PC's alignment to,subtract,add,PC's alignment cannot be greater,PC's alignment cannot be less than} #1 ?1{,,from PC's alignment,to PC's alignment}" );
            textResult.Add( "ce", @"start the character editor on the NPC in passive mode (this will terminate dialog)" );
            textResult.Add( "co", @"start combat between speakers and terminate dialog" );
            textResult.Add( "et", @"Tests whether PC can have expert training in skill #1 (&S1). If he can, then, continue dialog at #2. If he cannot, the NPC will say why and then dialog will continue at the response line for this line. " );
            textResult.Add( "fl", @"float line #1 above NPC's head and terminate dialog" );
            textResult.Add( "fp", @"give 1 fate point to the PC" );
            textResult.Add( "gf", @"!2{set,clear} global flag #1" );
            textResult.Add( "gv", @"set global variable #1 equal to value #2" );
            textResult.Add( "ii", @"start the inventory UI in identify mode" );
            textResult.Add( "in", @"transfer item with name index #1 from ?1{from PC or follower to NPC,from PC or follower to NPC,from NPC to PC}" );
            textResult.Add( "jo", @"ask NPC to join PC's group. !1{Paying,Not paying} attention to charisma limits. If successful, continue dialog at #2. If unsuccessful, the NPC will say why and then dialog will continue at the response line for this line." );
            textResult.Add( "lc", @"set local counter #1 equal to #2" );
            textResult.Add( "lf", @"!2{set,clear} local flag #1" );
            textResult.Add( "lv", @"make NPC leave the party" );
            textResult.Add( "mm", @"mark map area #1 as known on PC's map" );
            textResult.Add( "nk", @"kill the NPC involved in this dialog" );
            textResult.Add( "np", @"add newspaper #1 with !2{no,high} priority" );
            textResult.Add( "or", @"Set NPC's origin to #1" );
            textResult.Add( "pf", @"!2{set,clear} PC flag #1" );
            textResult.Add( "pv", @"set PC variable #1 equal to value #2" );
            textResult.Add( "qu", @"set quest #1 as being in state #2 (&Q2)" );
            textResult.Add( "re", @"?1{set NPC's reaction to PC to,set NPC's reaction to PC to,subtract,add,NPC's reaction to PC cannot be greater,NPC's reaction to PC cannot be less than} #1 ?1{,,from NPC's reaction to PC,to NPC's reaction to PC}" );
            textResult.Add( "ri", @"start the inventory UI in repair mode" );
            textResult.Add( "rp", @"?1{add,,remove} reputation #1 to PC" );
            textResult.Add( "rq", @"quell rumor #1" );
            textResult.Add( "ru", @"add rumor #1 to PC log" );
            textResult.Add( "sc", @"NPC will stay close" );
            textResult.Add( "so", @"NPC will spread out" );
            textResult.Add( "ss", @"set the current story state to #1 if it is lower than #1" );
            textResult.Add( "su", @"start the schematic UI on the PC" );
            textResult.Add( "tr", @"set the training of skill #1 (&S1) to #2 (&D2)" );
            textResult.Add( "uw", @"ask NPC to unwait and rejoin PC's group (assumes NPC was told to wait). !1{Paying,Not paying} attention to charisma limits. If successful, continue dialog at #2. Else and dialog will continue at the response line for this line." );
            textResult.Add( "wa", @"make the NPC wait here" );
            textResult.Add( "xp", @"award experience points to the PC as if he had solved a quest of level #1" );
#endif


            // �v����ɂP�F�P�ł͂Ȃ����\�̂悤�ɂP�F���ɂȂ�\�����E�E�E�\��male-female�̑Ή�������i����m2m�łP�F�P�ł��������ł����邪
            // rumors �� game_rd_npc_*.mes�̂����ꂩ�Ȃ̂����AID�ƈ�v���Ă��Ȃ��̂ň��p�ł��Ȃ��ǂ����o�R���Ȃ��ƑʖڂȂ̂��H
            string[] paths = {
                @"Arcanum\mes\logbk_ui.mes", // �N�G�X�g���
                @"Arcanum\mes\skill.mes", // sk
                @"Arcanum\mes\spell.mes", // sp
                @"Arcanum\mes\stat.mes", // race
                @"Arcanum\mes\gd_cls_pc2m.mes", // generated dialog class pc to male npc
                @"Arcanum\mes\gd_npc_m2m.mes", // generated dialog male npc to male pc
                @"Arcanum\mes\gd_pc2m.mes", // generated dialog  pc to male npc
                @"Arcanum\mes\gd_rce_m2m.mes", // generated dialog race male npc to male pc
                @"Arcanum\mes\gd_sto_m2m.mes", // generated dialog story state male npc to male pc
                @"Arcanum\modules\Arcanum\mes\game_rd_npc_m2m.mes", // rumor male npc to male pc
                @"Arcanum\modules\Arcanum\mes\game_rp_npc_m2m.mes", // reputations male npc to male pc
                @"Arcanum\modules\Arcanum\mes\gamearea.mes",
                @"Arcanum\modules\Arcanum\mes\gamenewspaper.mes",
                @"Arcanum\modules\Arcanum\mes\gamequestlog.mes", // q*
                @"Arcanum\modules\Arcanum\mes\gamereplog.mes", // reputation
                @"Arcanum\modules\Arcanum\mes\StoryState.mes",
                @"Arcanum\modules\Arcanum\semes\globalflags.mes", //gf
                @"Arcanum\modules\Arcanum\semes\globalvars.mes", //gv
                @"Arcanum\oemes\oname.mes", // using pa,in,ni? name index
                @"Arcanum\rules\xp_quest.mes", // xp
            };

            // xp�ŎQ�Ƃ���̂�xp_quest.mes ������Ȃ̂��ȁH
            //value.mes �ɕϐ�������
            
            foreach ( string path in paths ) {
                if ( load( path ) == false ) {
                    // failed load
                    string err = "[ERROR] " + @"can't open: " + path;
                    System.Console.WriteLine( err );
                }
            }
        }

        bool load( string path ) {
            
            File file = new File();
            string fullpath = Path.Data.ModifiedDirectory + path;
            if ( file.open( fullpath, false ) == false ) {
                //fullpath = Path.convineFullPath( path );
                fullpath = path;
                if ( file.open( fullpath, true ) == false ) {
                    return false;
                }
            }
            string name = System.IO.Path.GetFileNameWithoutExtension( fullpath );
            Mes.Add( name, file );
            return true;

        }
        string getText( string id, string name ) {
            File mes = Mes[ name ] as File;
            if ( mes != null ) {
                string key = removeSign( id );
                File.Chunk chunk = mes.Chunks[ key ] as File.Chunk;
                if ( chunk != null ) {
                    return chunk.Message.Original;
                }
            }
            return null;
        }
        string getNodeText( string id, string name ) {
            File mes = Mes[ name ] as File;
            if ( mes != null ) {
                string key = removeSign( id );
                File.Chunk chunk = mes.Chunks[ key ] as File.Chunk;
                if ( chunk != null ) {
                    return chunk.NodeText;
                }
            }
            return null;
        }

        static public string removeSign( string name ) {
            if ( name != null && name.Length > 0 ) {
                int index = 0;
                // �擪�������ȊO�̂Ȃ�炩�̕��� �p�[�X����whitespace��菜����Ă���̂�trim�͕s�v
                if ( char.IsNumber( name[ 0 ] ) == false ) {
                    index = 1; // �߂�ǂ��̂ňꕶ���ȏ�͒T���Ȃ�
                }
#if false
                switch ( name[ 0 ] ) {
                    case '0': // 0�͂��肦��͂����ȔO�̂��ߏ��O
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                    default:
                        index = 1; // �߂�ǂ��̂ňꕶ���ȏ�͒T���Ȃ�
                        break;
                }
#endif
                return name.Substring( index ); // 0�̏ꍇ�ł����Əd���H
            }
            return null;
        }

        // �g�[�N��#?{},
        // �ŒP���ɏ��������������̊֐��ƁAmes�A�N�Z�X���K�v�ȕ��̊֐���
        // &������͖��������������Ă��Ă�Ȃ�

        // ��{��!1�ł����Ƃ����?1�����Ă����肵�ď����̎d�l���B������
        // >= <=��!1�ł����̂�?1�ɂȂ��Ă�Ƃ��A��+�Ȃ��ꍇ��>,,<��" "�ɂȂ��Ă��܂����A����>�������������!1�̎d�l��

        // !��0,1��?��-1,0,1���Ă����������ƒ�`���Ă���̂�
        // ���ꂾ��>= �� >,>,<���d�l�I�ɐ������f�[�^���낤

        // resID�Ń}�C�i�X�t���̂����邪�A��Βl�ōl���Ė��Ȃ������炵���B

        // �d�l�ύX����&[A-Z]1|2��%1|2�ɂ��܂�

        // &����serialize�̍ۂ�&amp;�łȂ��ƑʖڂɂȂ�A��Ȃ��̂�%�ɕύX

        // !1   {1�ȏ�,0�ȉ�}�ɒu��
        // ?1   {+,��������,-} or {+,��������,-,>,<}�ɒu��
        // #1   �ϐ����Βl�ɒu��
        // %1   ���l�������mes��ID���Q�Ƃ��Ēu��

        // �ėp
        // ������Q�Ƃ�����Ȃ���͂��ꂾ���ł����������H
        string test( string key, List<string> args, File.Node sender ) {
            string text = textTest[ key ] as string;

            // #1,#2�̉���
            text = replace( text, args );
            text = sign( text, args );
            text = bit( text, args );

            return text;

        }

        string testArea( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "gamearea" );
                // "/"��split���Ȃ��Ǝg���Ȃ�
                // gettext����message�ł���
                if ( s != null ) {
                    string[] split = s.Split( '/' );
                    // �ʏ�͎O�ɂȂ�͂������K�v�Ȃ͓̂��
                    if ( split.Length > 2 ) {
                        text = replace( text, "%1", split[ 1 ] );
                    }
                }
            }
            return text;
        }
        
        string testGlobalFlags( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "globalflags" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }
        string testGlobalVars( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "globalvars" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string testNameIndex( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "oname" );
                if( s != null ) {
                    text = replace( text, "%1", s );
                }
            }
            return text;
        }

        string testSkill( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 1 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "skill" );
                if ( s != null ) {
                    text = replace( text, "%1", s );
                }
            }
            return text;
        }
        
        string testSpellCollege( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 1 ) {
                string arg = args[ 0 ];
                int id = 0;
                if ( toInt32( removeSign( arg ), out id ) ) {
                    id += 500; // offset 500
                    string s = getText( id.ToString(), "spell" );
                    if ( s != null ) {
                        text = replace( text, "%1", s );
                    }
                }
            }
            return text;
        }


        string testQuest( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 1 ) {
                {
                    string arg = args[ 0 ];
                    string s = getNodeText( arg, "gamequestlog" );
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
                // 22��Accepted�͓����ł͊������Ă��邪���񍐂̃N�G�X�g�炵��
                // �Ȃ̂ŋ�ʗp��text�ł͂Ȃ�nodetext���Ƃ�
                int id = 0;
                if ( toInt32( removeSign( args[ 1 ] ), out id ) ) {
                    id += 19; // offset 19
                    string s = getNodeText( id.ToString(), "logbk_ui" );
                    if ( s != null ) {
                        text = replace( text, "%2", s );
                    }
                }
            }
            // 2�ŃX�e�[�g�Ƃ��Ă���
            return text;
        }

        string testRace( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                int id = 0;
                if ( toInt32( removeSign( arg ), out id ) ) {
                    id += 30; // offset 30
                    string s = getText( id.ToString(), "stat" );
                    if ( s != null ) {
                        text = replace( text, "%1", s );
                    }
                }
            }
            return text;
        }
        
        string testReputation( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "gamereplog" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            // 2�ŃX�e�[�g�Ƃ��Ă���
            return text;
        }

        string testRumor( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                int id = 0;
                if ( toInt32( removeSign( arg ), out id ) ) {
                    id *= 20; // rumor��20�̔{���ŋ��߂���
                    string s = getNodeText( id.ToString(), "game_rd_npc_m2m" );
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
            }
            return text;
        }

        string testStoryState( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "StoryState" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            // 2�ŃX�e�[�g�Ƃ��Ă���
            return text;
        }

        string testTraining( string key, List<string> args, File.Node sender ) {
            string text = test( key, args, sender );
            if ( args.Count > 1 ) {
                {
                    string arg = args[ 0 ];
                    string s = getText( arg, "skill" );
                    if ( s != null ) {
                        text = replace( text, "%1", s );
                    }
                }

                int id = 0;
                if ( toInt32( removeSign( args[ 1 ] ), out id ) ) {
                    id += 16; // offset 16
                    string s = getText( id.ToString(), "skill" );
                    if ( s != null ) {
                        text = replace( text, "%2", s );
                    }
                }
            }
            return text;
        }


        string result( string key, List<string> args, File.Node sender ) {
            string text = textResult[ key ] as string;
            // #1,#2�̉���
            text = replace( text, args );
            text = sign( text, args );
            text = bit( text, args );
            return text;
        }

        string resultExpertTraining( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 1 ) {
                {
                    string arg = args[ 0 ];
                    string s = getText( arg, "skill" );
                    if ( s != null ) {
                        text = replace( text, "%1", s );
                    }
                }
#if false // �ʃm�[�h�ɂԂ炳����̂łƂ肠�����s�v��

                {
                    string arg = args[ 1 ];
                    string sub = removeSign( arg );
                    if ( sender.Parent.Chunks.ContainsKey( sub ) ) {
                        File.Chunk chunk = sender.Parent.Chunks[ sub ] as File.Chunk;
                        string s = chunk.NodeText;
                        if ( s != null ) {
                            text += "\n" + s;
                        }
                    }
                }
#endif
            }
            return text;
        }

        string resultFloatLine( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
#if false // �ʃm�[�h�ɂԂ炳����̂łƂ肠�����s�v��
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string sub = removeSign( arg );
                if ( sender.Parent.Chunks.ContainsKey( sub ) ) {
                    File.Chunk chunk = sender.Parent.Chunks[ sub ] as File.Chunk;
                    string s = chunk.NodeText;
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
            }
#endif
            return text;
        }

        string resultGlobalFlags( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "globalflags" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string resultGlobalVars( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "globalvars" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string resultJoin( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
#if false // �ʃm�[�h�ɂԂ炳����̂łƂ肠�����s�v��
            if ( args.Count > 1 ) {
                string arg = args[ 1 ];
                string sub = removeSign( arg );
                if ( sender.Parent.Chunks.ContainsKey( sub ) ) {
                    File.Chunk chunk = sender.Parent.Chunks[ sub ] as File.Chunk;
                    string s = chunk.NodeText;
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
            }
#endif
            return text;
        }

        string resultMarkMap( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "gamearea" );
                // "/"��split���Ȃ��Ǝg���Ȃ�
                // gettext����message�ł���
                if ( s != null ) {
                    string[] split = s.Split( '/' );
                    // �ʏ�͎O�ɂȂ�͂������K�v�Ȃ͓̂��
                    if ( split.Length > 2 ) {
                        text = replace( text, "%1", split[ 1 ] );
                    }
                }
            }
            return text;
        }

        string resultNameIndex( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "oname" );
                text = replace( text, "%1", s );
            }
            return text;
        }

        string resultNewspaper( string key, List<string> args, File.Node sender ) {
            // ��̈����̂������ł����A�f�t�H���g�l�����邩�N�G�X�g�p�Ȃ̂���
            string text = result( key, args, sender );
            if ( args.Count > 1 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "gamenewspaper" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string resultQuest( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 1 ) {
                {
                    string arg = args[ 0 ];
                    string s = getNodeText( arg, "gamequestlog" );
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
                // 22��Accepted�͓����ł͊������Ă��邪���񍐂̃N�G�X�g�炵��
                // �Ȃ̂ŋ�ʗp��text�ł͂Ȃ�nodetext���Ƃ�
                int id = 0;
                if ( toInt32( removeSign( args[ 1 ] ), out id ) ) {
                    id += 19; // offset 19
                    string s = getNodeText( id.ToString(), "logbk_ui" );
                    if ( s != null ) {
                        text = replace( text, "%2", s );
                    }
                }
            }
            return text;
        }

        string resultStoryState( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "StoryState" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string resultReputation( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getNodeText( arg, "gamereplog" );
                if ( s != null ) {
                    text += "\n" + s;
                }
            }
            return text;
        }

        string resultRumor( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                int id = 0;
                if ( toInt32( removeSign( arg ), out id ) ) {
                    id *= 20; // rumor��20�̔{���ŋ��߂���
                    string s = getNodeText( id.ToString(), "game_rd_npc_m2m" );
                    if ( s != null ) {
                        text += "\n" + s;
                    }
                }
            }
            return text;
        }

        string resultTraining( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 1 ) {
                {
                    string arg = args[ 0 ];
                    string s = getText( arg, "skill" );
                    if ( s != null ) {
                        text = replace( text, "%1", s );
                    }
                }

                int id = 0;
                if ( toInt32( removeSign( args[ 1 ] ), out id ) ) {
                    id += 16; // offset 16
                    string s = getText( id.ToString(), "skill" );
                    if ( s != null ) {
                        text = replace( text, "%2", s );
                    }
                }
            }
            return text;
        }

        string resultXP( string key, List<string> args, File.Node sender ) {
            string text = result( key, args, sender );
            if ( args.Count > 0 ) {
                string arg = args[ 0 ];
                string s = getText( arg, "xp_quest" );
                text = replace( text, "%1", s );
            }
            return text;
        }

        string dialog( string key, List<string> args, File.Node sender ) {
            string text = textDialog[ key ] as string;
            return text;
        }

        string dialogDirections( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogGreeting( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogMoney( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogQuests( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogRumors( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogTraining( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogUseskill( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogXMarkstheSpot( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }
        string dialogZapspell( string key, List<string> args, File.Node sender ) {
            string text = dialog( key, args, sender );
            return text;
        }

        // result��re��6����₪�����ĈӖ��s�� �����łȂ��Đ��l�̑召�Ȃ̂��ȁ[�U�i�K�́H
        // ����ȊO�͐��������Ȃ̂���
        // substruct, set, add�͕����邪�A����ȊO�̃P�[�X���݂����
        // <, >�̕���������炵��
        // ����ł��P�������A��
        // �V�����^�ł����������������ł�%�Ƃ�$�Ƃ�
        // <>������Ő��l�ɕϊ����Ă����̂��ˁ[
        // ��������炢�g�[�N�������肻���Ȃ̂����A�ǂꂾ�낤

        // re
        // al���ŏ�6�Ō��4��
        string replace( string text, string token, string word ) {
            return text.Replace( token, word );
        }

        string replace( string text, List<string> args ) {
            string[] tokens = { "#1", "#2" };
            for ( int i = 0; i < tokens.Length; ++i ) {
                if ( args.Count > i ) {
                    string arg = args[ i ];
                    if ( arg.Length > 0 ) {
                        int start = 0;
                        // +-�������Ȃ��Ƃȁ[�ŏ�����ʂȂ�y�Ȃ񂾂�
                        switch ( arg[ 0 ] ) {
                            case '-':
                            case '+':
                            case '<':
                            case '>':
                                start = 1;
                                break;
                        }
                        text = text.Replace( tokens[ i ], arg.Substring( start ) );
                    }
                }
            }
            return text;
        }

        string sign( string text, List<string> args ) {
            string[] tokens = { "?1", "?2" };
            for ( int t = 0; t < tokens.Length; ++t ) {
                int start = 0;
                // �s���f�[�^�̖������[�v�p�ɉ񐔐�������H
                while ( true ) {
                    int pos = text.IndexOf( tokens[ t ], start );
                    if ( pos < 0 ) {
                        break;
                    }
                    //int index = pos + @"?1".Length;
                    int index = pos;
                    int s = -1;
                    int e = -1;
                    //int offset = 0; // �u�����ړ�
                    int offset = tokens[ t ].Length; // �u�����ړ�
                    for ( int i = index; i < text.Length; ++i ) {
                        char c = text[ i ];
                        switch ( c ) {
                            case '{':
                                s = i + 1;
                                break;
                            case '}':
                                e = i;
                                break;
                        }
                        if ( s >= 0 && e >= 0 ) {
                            string sub = text.Substring( s, ( e - s ) );
                            string[] split = sub.Split( ',' );
                            string arg = args[ t ];
                            if ( arg.Length > 0 ) {
                                int sign = 1;
                                switch ( arg[ 0 ] ) {
                                    case '-':
                                        sign = 2;
                                        break;
                                    case '+':
                                        sign = 0;
                                        break;
                                    case '>':
                                        sign = 3;
                                        break;
                                    case '<':
                                        sign = 4;
                                        break;
                                }
                                // re�ɍl�����Ă݂� ����Ă�̂Ŏd�l��ύX���Ĕėp�����Ȃ���
                                if ( split.Length > sign ) {
                                    string rep = split[ sign ];
                                    offset = rep.Length;
                                    string old = text.Substring( pos, ( e + 1 ) - pos );
                                    text = text.Replace( old, rep );
                                }
                            }
                            //text.Remove();
                            s = -1;
                            e = -1;
                            break; // �΂ɂȂ�{}�����������̂ł����
                        }
                    }
                    start = index + offset; // �u�����Ă��Ȃ��Ƃ͂܂�Ȃ�
                    // �Ƃ������Ƃŏ����l�͒u���ł��Ȃ������ꍇ�̈ړ��������ɂ��Ă���
                    // ��������break����Ă��Ȃ����_�Ō��������Ƃ������Ƃ��낤
                }
            }
            return text;
        }
        string bit( string text, List<string> args ) {
            string[] tokens = { "!1", "!2" };
            for ( int t = 0; t < tokens.Length; ++t ) {
                int start = 0;
                // �s���f�[�^�̖������[�v�p�ɉ񐔐�������H
                while ( true ) {
                    int pos = text.IndexOf( tokens[ t ], start );
                    if ( pos < 0 ) {
                        break;
                    }
                    //int index = pos + @"?1".Length;
                    int index = pos;
                    int s = -1;
                    int e = -1;
                    //int offset = 0; // �u�����ړ�
                    int offset = tokens[ t ].Length; // �u�����ړ�
                    for ( int i = index; i < text.Length; ++i ) {
                        char c = text[ i ];
                        switch ( c ) {
                            case '{':
                                s = i + 1;
                                break;
                            case '}':
                                e = i;
                                break;
                        }
                        if ( s >= 0 && e >= 0 ) {
                            string sub = text.Substring( s, ( e - s ) );
                            string[] split = sub.Split( ',' );
                            string arg = null;
                            if ( t < args.Count ) {
                                arg = args[ t ];
                            } else {
                                // �����ꍇ�A�ǂꂩ�Ɠ����Ɍ����ق����悢�̂ł́H
                                // ���̊֐��ł����l�̂��Ƃ͂��邩��
                                arg = "1"; // �Ƃ肠����
                            }
                            if ( arg.Length > 0 ) {
                                int num = -1;
                                //int sign = 1;
                                if ( toInt32( arg, out num ) ) {
                                    //if ( num > 0 ) {
                                    //    sign = 0;
                                    //}
                                }

                                if ( split.Length == 2 ) {
                                    string rep = split[ num ];
                                    offset = rep.Length;
                                    string old = text.Substring( pos, ( e + 1 ) - pos );
                                    text = text.Replace( old, rep );
                                }
                            }
                            //text.Remove();
                            s = -1;
                            e = -1;
                            break; // �΂ɂȂ�{}�����������̂ł����
                        }
                    }
                    start = index + offset; // �u�����Ă��Ȃ��Ƃ͂܂�Ȃ�
                    // �Ƃ������Ƃŏ����l�͒u���ł��Ȃ������ꍇ�̈ړ��������ɂ��Ă���
                    // ��������break����Ă��Ȃ����_�Ō��������Ƃ������Ƃ��낤
                }
            } 
            return text;
        }

        // re�ɍl�����Ă݂���
        static public bool toInt32( string s, out int num ) {
            if ( s != null && s.Length > 0 ) {
                int index = 0;
                switch ( s[ 0 ] ) {
                    case '>':
                    case '<':
                        index = 1;
                        break;
                }
                string sub = s.Substring( index );

                return Int32.TryParse( sub, out num );
            }
            num = 0;
            return false;
        }

        // �t�����\���ǂ����A�\�Ȃ炻�̎g���ׂ�������Ԃ�
        static public string isReference( KeyArguments ka ) {
            // �f�[�^�h���u���ɂ�������
            switch ( ka.Key ) {
            case "fl":
                // arg0
                if ( ka.Args.Count > 0 ) {
                    return ka.Args[ 0 ];
                }
                break;
            case "et":
            case "jo":
            case "uw":
                //arg1
                if ( ka.Args.Count > 1 ) {
                    return ka.Args[ 1 ];
                }
                break;
            default:
                // null���܂�
                break;
            }
            return null;
        }

        static public KeyArguments getGeneratedDialog( string line ) {
            KeyArguments ka = null;
            if( File.checkAvailable( line ) == false ) {
                // ��̏ꍇ������̂�
                if( line != null && line.Length > 0 ) {
                    string[] split = line.Split( ':' );
                    if( split != null && split.Length > 0 ) {
                        // one letter key
                        if( split[ 0 ].Length == 1 ) {
                            string key = split[ 0 ];
                            string[] args = null;
                            // ������:���o�Ă��邱�Ƃ͖����Ǝv�����c
                            if( split.Length > 1 ) {
                                System.Diagnostics.Debug.Assert( split.Length == 2 );
                                args = split[ 1 ].Split( ',' );
                            }
                            // add
                            ka = new KeyArguments();
                            ka.Key = key;
                            if( args != null ) {
                                ka.Args.AddRange( args );
                            }
                        }

                    }
                }
            }
            return ka;
        }

        static public List<KeyArguments> getKeyArguments( string line ) {
            List<KeyArguments> list = new List<KeyArguments>();
            bool isKey = true;
            bool isArg = false;
            string arg = "";
            KeyArguments ka = new KeyArguments();
            for ( int i = 0; i < line.Length; ++i ) {
                char c = line[ i ];
                switch ( c ) {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    if ( isKey ) {
                        ka.Key = arg.Clone() as string;
                        arg = "";
                    }
                    isKey = false;
                    isArg = true;
                    arg += c;
                    break;
                case '+':
                case '-':
                case '<': // �v����
                case '>': // �v����
                    // ��݂Ƃ�Œ��ɂł��̂Ȃ�΋�؂�
                    if ( isKey ) {
                        ka.Key = arg.Clone() as string;
                        arg = "";
                    } else if ( isArg ) {
                        ka.Args.Add( arg.Clone() as string );
                        arg = "";
                    }
                    isKey = false;
                    isArg = true;
                    arg += c; // �Ȃ񂩂����i�[�Ȃ��̂���
                    break;
                case ',':
                    // �Ƃɂ�����؂�
                    if ( isKey ) {
                        ka.Key = arg.Clone() as string;
                    } else if ( isArg ) {
                        ka.Args.Add( arg.Clone() as string );
                    }
                    list.Add( ka );
                    ka = new KeyArguments();
                    arg = "";
                    isKey = false;
                    isArg = false;
                    break;
                case '\t':
                case ' ':
                    // ��݂Ƃ�Œ��ɂł��̂Ȃ�΋�؂�
                    // key �̏ꍇ�͎��ɏo�镶���ŋ�؂���̂ł���Ȃ����H
                    //if ( isKey ) {
                    //    ka.Key = key.Clone();
                    //    arg = "";
                    //    isKey = false;
                    //} else
                    if ( isArg ) {
                        ka.Args.Add( arg.Clone() as string );
                        arg = "";
                        isArg = false;
                    }
                    break;
                default:
                    // �V�����L�[�Ȃ̂ŋ�؂�
                    if ( isArg ) {
                        ka.Args.Add( arg.Clone() as string );
                        list.Add( ka );
                        ka = new KeyArguments();
                        arg = "";
                        isKey = false;
                        isArg = false;
                    }
                    isKey = true;
                    arg += c;
                    break;
                }
            }
            // amari
            if ( isKey || isArg ) {
                if ( isKey ) {
                    ka.Key = arg.Clone() as string;
                } else if ( isArg ) {
                    ka.Args.Add( arg.Clone() as string );
                }
                list.Add( ka );
            }

#if false // debug
            Console.WriteLine( "parse[" + line + "]" );
            int ki = 0;
            foreach ( KeyArguments k in list ) {
                string debug = "key" + ki++ + "[" + k.Key + "]";
                int ai = 0;
                foreach ( string a in k.Args ) {
                    debug += " arg" + ai++ + "[" + a + "]";
                }
                Console.WriteLine( debug );
            }
#endif
            // check invalid
            foreach ( KeyArguments k in list ) {
                if ( k.Key.Length > 0 ) {
                    return list;
                }
                foreach ( string a in k.Args ) {
                    if ( a.Length > 0 ) {
                        return list;
                    }
                }
            }
            //return list;
            return null;
        }

        static public string getValue( File.Node node, KeyArguments ka, out string refid ) {
            // ���񎞂̂�
            if ( instance == null ) {
                instance = new Parser( Path.Data.ModifiedDirectory );
            }
            refid = null;
            if ( node == null ) {
                return "ERROR";
            }
            string ret = null;
            switch ( node.Type ) {
            case File.Node.NodeType.MaleLine:
            case File.Node.NodeType.FemaleLine:
                // delegete
                if( ka.Key.Length > 0 ) {
                    GetValue func = instance.funcsDialog[ ka.Key.ToUpper() ] as GetValue;
                    if( func != null ) {
                        ret = instance.textDialog[ ka.Key.ToUpper() ] as string;
                        //ret = func( ka.Key.ToLower(), ka.Args, node );
                    } else {
                        // error log
                        string err = "[ERROR] " + node.Original + " : " + ka.Key;
                        System.Console.WriteLine( err );
                        System.Diagnostics.Debug.Assert( false, err );
                    }
                }
                break;
            case File.Node.NodeType.IntCheck:
                if ( ka.Args.Count > 0 ) {
                    string arg = ka.Args[ 0 ];
                    int req = -1;

                    if ( toInt32( arg, out req ) ) {
                        if ( req < 0 ) {
                            ret = arg.Substring( 1 ) + " �ȉ�";
                        } else {
                            ret = arg + " �ȏ�";
                        }
                    }
                }
                break;
            case File.Node.NodeType.TestCodes:
                // delegete
                if ( ka.Key.Length > 0 ) {
                    GetValue func = instance.funcsTest[ ka.Key.ToLower() ] as GetValue;
                    if ( func != null ) {
                        ret = func( ka.Key.ToLower(), ka.Args, node );
                    } else {
                        // error log
                        string err = "[ERROR] " + node.Original + " : " + ka.Key;
                        System.Console.WriteLine( err );
                        System.Diagnostics.Debug.Assert( false, err );
                    }
                }
                break;
            case File.Node.NodeType.ResponseID:
                if ( ka.Args.Count > 0 ) {
                    string arg = ka.Args[ 0 ];
                    if ( arg.Length > 0 ) {
                        //string sub = removeSign( arg );
                        // �}�C�i�X�̓X�N���v�g�̔ԍ��փW�����v
                        string sub = arg;
                        if ( node.Parent.Chunks.ContainsKey( sub ) ) {
                            File.Chunk chunk = node.Parent.Chunks[ sub ] as File.Chunk;
                            ret = chunk.NodeText;
#if false
                            switch ( ka.Key ) {
                            case "fl":
                                if ( ka.Args.Count > 0 ) {
                                    refid = ka.Args[ 0 ];
                                }
                                break;
                            case "et":
                            case "jo":
                            case "uw":
                                if ( ka.Args.Count > 1 ) {
                                    refid = ka.Args[ 1 ];
                                }
                                break;
                            }
#endif
                            refid = sub;
                        } else {
                            ret = sub;
                        }

                    }
                }
                break;
            case File.Node.NodeType.Result:
                // delegete
                if ( ka.Key.Length > 0 ) {
                    GetValue func = instance.funcsResult[ ka.Key.ToLower() ] as GetValue;
                    if ( func != null ) {
                        ret = func( ka.Key.ToLower(), ka.Args, node );
                        refid = isReference( ka );
                        //refid = Parser.removeSign( ret ); // ��Βl

                    } else {
                        // error log
                        string err = "[ERROR] " + node.Original + " : " + ka.Key;
                        System.Console.WriteLine( err );
                        System.Diagnostics.Debug.Assert( false, err );
                    }
                }
                break;
            default:
                break;
            }

            if ( ret != null ) {
                // ����prefix���܂܂�Ă���̂ł���Ȃ�
                if( node.Type == File.Node.NodeType.ResponseID ) {
                    return ret;
                }
                // prefix�t�^ {}����ID�ƕ���킵������
                string result = "[";
                if ( ka.Key != null && ka.Key.Length > 0 ) {
                    result += ka.Key;
                }
                foreach ( string arg in ka.Args ) {
                    // [�ȊO�ɕ����񂪒ǉ�����Ă���ꍇ�͋�؂�
                    if ( result.Length > 1 ) {
                        result += ", ";
                    }
                    result += arg;
                }
                result += "]";
                result += " " + ret;
                return result;
            }
            return null;
        }
#if false // old method
        static public string getValue( File.Node node ) {
            // ���񎞂̂�
            if ( instance == null ) {
                instance = new Parser( Path.Data.ModifiedDirectory );
            }
            if ( node == null ) {
                return "ERROR";
            }
            // ,�ŋ�؂��Ă��Ȃ��P�[�X������񂾂��A�o�O�ł́H
            // �擪���珈�����Ă�����key���܂�Ɠǂݍ��ވ��������肵�Ă���̂��H
            string[] split = node.Original.Split( ',' );
            if ( split.Length > 0 ) {
                string result = "";
                foreach ( string sp in split ) {
                    string key = "";
                    string val = "";
                    bool isValue = false;
                    List<string> args = new List<string>();
                    for ( int i = 0; i < sp.Length; ++i ) {
                        char c = sp[ i ];
                        switch ( c ) {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                isValue = true;
                                val += c; // �Ȃ񂩂����i�[�Ȃ��̂���
                                break;
                            case '+':
                            case '-':
                            case '<': // �v����
                            case '>': // �v����
                                // �����Œ��ɂł��̂Ȃ�΋�؂�
                                if ( isValue ) {
                                    args.Add( val.Clone() as string );
                                    val = "";
                                }
                                isValue = true;
                                val += c; // �Ȃ񂩂����i�[�Ȃ��̂���
                                break;
                            case '\t':
                            case ' ':
                                // �����Œ��ɂł��̂Ȃ�΂���͋�؂�
                                if ( isValue ) {
                                    args.Add( val.Clone() as string );
                                    val = "";
                                }
                                isValue = false;
                                //�������ɏo��ΈӖ�������
                                break;
                            default:
                                // ���O�͓�Ƃ��o�Ȃ��̂���
                                key += c; // �Ȃ񂩂����i�[�Ȃ�����
                                break;
                        }
                    }

                    // ���܂�����
                    if ( val.Length > 0 ) {
                        args.Add( val.Clone() as string );
                    }
                    if ( key.Length > 0 ) {
                        key = key.ToLower();
                    }

                    // ���f����
                    string ret = null;
                    // �f���Q�[�g�ł��������߂�ǂ��ł���
                    switch ( node.Type ) {
                        case File.Node.NodeType.IntCheck:
                            if ( args.Count > 0 ) {
                                string arg = args[ 0 ];
                                int req = -1;

                                if ( toInt32( arg, out req ) ) {
                                    if ( req < 0 ) {
                                        ret = arg.Substring( 1 ) + " �ȉ�";
                                    } else {
                                        ret = arg + " �ȏ�";
                                    }
                                }
                            }
                            break;
                        case File.Node.NodeType.TestCodes:
                            // delegete�����܂�
                            if ( key.Length > 0 ) {
                                GetValue func = instance.funcsTest[ key ] as GetValue;
                                if ( func != null ) {
                                    ret = func( key, args, node );
                                } else {
                                    // error log
                                    string err = "[ERROR] " + node.Original + " : " + key;
                                    System.Console.WriteLine( err );
                                    System.Diagnostics.Debug.Assert( false, err );
                                }
                            }
                            break;
                        case File.Node.NodeType.ResponseID:
                            // �ނ���args�����Ă�����
                            if ( args.Count > 0 ) {
                                string arg = args[ 0 ];
                                if ( arg.Length > 0 ) {
                                    string sub = removeSign( arg );
                                    if ( node.Parent.Chunks.ContainsKey( sub ) ) {
                                        File.Chunk chunk = node.Parent.Chunks[ sub ] as File.Chunk;
                                        ret = chunk.NodeText;
                                    }

                                }
                            }
                            break;
                        case File.Node.NodeType.Result:
                            // delegete�����܂�
                            if ( key.Length > 0 ) {
                                GetValue func = instance.funcsResult[ key ] as GetValue;
                                if ( func != null ) {
                                    ret = func( key, args, node );
                                } else {
                                    // error log
                                    string err = "[ERROR] " + node.Original + " : " + key;
                                    System.Console.WriteLine( err );
                                    System.Diagnostics.Debug.Assert( false, err );
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if ( result.Length > 0 ) {
                        result += "\n";
                        result += "\n";
                    }
                    if ( ret != null ) {
                        result += "[" + sp.Trim() + "]  " + ret;
                    } else {
                        result += sp.Trim();
                    }

                }

                if ( result.Length > 0 ) {
                    return result;
                }
            }

            return node.Original;
        }
#endif

#if false
        // �l�̕]�����Ď������ɍs���̂�������̂ŁA����File.Node�����肪����
        public static string getGeneratedDialog( string line ) {
            // parser�Ɉڂ��������悢�H
            if ( File.checkAvailable( line ) ) {
                return null;
            }
            if ( instance == null ) {
                instance = new Parser( Path.Data.ModifiedDirectory );
            }

            string t = line.Trim();
            string ret = null;
            if ( t != null && t.Length > 0 ) {
            
                if ( t.Length == 1 ) {
                    // male or femle
                    // �f�[�^�h���u���ɂ��Ă�������
                    switch ( t[ 0 ] ) {
                        case '0':
                            ret = "PC�������̎��݂̂ɏo�������b��";
                            break;
                        case '1':
                            ret = "PC���j���̎��݂̂ɏo�������b��";
                            break;
                        default:
                            // error
                            break;
                    }
                } else if ( t.Length > 1 && char.IsUpper( t[ 0 ] ) && t[ 1 ] == ':' ) {
                    // ,��؂�̈������߂��ς܂��Ă���
                    string sub = t.Substring( 2 );
                    sub = sub.Trim();
                    string[] args = null;
                    // �������Ȃ��ꍇ��null
                    if ( sub.Length > 0 ) {
                        args = sub.Split( ',' );
                    }

                    ret = "Generated Dialog"; // test

                    // map�ɂ��ăf�[�^�h���u���ɂ��Ă�������
                    switch ( t[ 0 ] ) {
                        case 'A':
                            break;
                        case 'B':
                            break;
                        case 'C':
                            break;
                        case 'D':
                            break;
                        case 'E':
                            break;
                        case 'F':
                            break;
                        case 'G':
                            break;
                        case 'H':
                            break;
                        case 'I':
                            break;
                        case 'J':
                            break;
                        case 'K':
                            break;
                        case 'L':
                            break;
                        case 'M':
                            break;
                        case 'N':
                            break;
                        case 'O':
                            break;
                        case 'P':
                            break;
                        case 'Q':
                            break;
                        case 'R':
                            break;
                        case 'S':
                            break;
                        case 'T':
                            break;
                        case 'U':
                            break;
                        case 'V':
                            break;
                        case 'W':
                            break;
                        case 'X':
                            break;
                        case 'Y':
                            break;
                        case 'Z':
                            break;
                        default:
                            // error
                            break;
                    }
                }
            }
            return ret;
        }
#endif
    }
}
