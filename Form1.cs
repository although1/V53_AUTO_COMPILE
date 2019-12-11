using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace V53_AUTO_COMPILE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] names = new string[] {
                "#export CC=/usr/bin/gcc\n",
                "#在编译之前要确定的东西，首先要制定好customer，logo，IR\n",
                "#最后本脚本一共有七处需要手动修改，有些可以不改，加了*的是必改\n",
                "#在编译之前要先生成存放软件的目录，请先看‘需要改一’\n",
                "\n",
                "#需要改一 \n",
                "#OUT_BIN_PATH：这个是生成在根目录的文件夹，里面存放软件，可以不改\n",
                "OUT_BIN_PATH=./YDG_SOFTWARE\n",
                "OUT_BIN_DIR=",textBox2.Text,"\n",
                "\n",
                "#需要改二 *\n",
                "#OUT_BIN_NAME：这个是生成软件的文件名\n",
                "#WORK_PATH：是你代码的根目录\n",
                "OUT_BIN_NAME=YDG.V53.bin\n",
                "WORK_PATH=",textBox1.Text,"\n",
                "CUSTOMER_DIR=",textBox3.Text,"\n",
                "\n",
                "#需要改三 *\n",
                "#以下四个是customize.h 中你建立好的customer，用于切换customize.h中的4:3,16:9\n",
                "CUSTOMER_SELECT=\"#define CUSTOMER_SELECT    \"\n",
                "C4_3COSTOMER_1=",textBox5.Text,"\n",
                "C4_3COSTOMER=$CUSTOMER_SELECT$C4_3COSTOMER_1\n",
                "\n",
                "C16_9COSTOMER_1=",textBox4.Text,"\n",
                "C16_9COSTOMER=$CUSTOMER_SELECT$C16_9COSTOMER_1\n",
                "\n",
                "CURRENT_CUSTOMER=$C4_3COSTOMER_1\n",
                "\n",
                "#需要改四 *\n",
                "#iConfigNumber：是最大屏参数，若当前最大为14，则填15\n",
                "iConfigNumber=",Get_Total_Panel_Number(),
                "\n\n",
                "#需要改五 *\n",
                "\n",
                "#i4_3Index：是16:9的第一个屏参的编号，用于切换customize.h中的4:3,16:9\n",
                "i4_3Index=",Get_4_3_Panel_Number(),
                "\n\n########################\n",
                "#\n",
                "# Generate Panel data\n",
                "#\n",
                "########################\n",
                "#############################################   4:3   #########################################\n",
                "\n",
                "#注意panel_name中的x按照屏参名来写\n",
                "\n",
                "PANEL_SELECT=\"#define PANEL_DEFAULT_TYPE_SEL					\"\n",
                "EDID_SELECT=\"#define PANEL_EDID_TYPE_SEL                   	\"\n",
                "\n",
            };
            string[] names1 = new string[] {
                "#############################################   16:9   #########################################\n\n",
            };

            string[] names2 = new string[] {
                "#=======================================================\n",
                "echo \"====================================================\"\n",
                "echo \"Start build YDG SOFTWARE \"\n",
                "\n",
                "function error_exit {\n",
                "  echo \"$1\" 1>&2\n",
                "  exit 1\n",
                "}\n",
                "#===================================================\n",
                "\n",
                "iBuildIndex=0\n",
                "iBuildIndex1=0\n",
                "iBuildIndex2=0\n",
                "\n",
                "echo \"====================================================\"\n",
                "echo \"Start build YDG SOFTWARE \"\n",
                "#需要改六 *\n",
                "#需要改的地方是里面的JEDIA,MIRROR_JEDIA,MIRROR_VESA,VESA\n",
                "#比如正常文件夹名为CSTM_KL_ENG_T_R5L_03C_05_GENERAL_1440x900_IRCVT00BF_KPD_LUCKY_JEDIA_20190417\n",
                "#则要改成	JEDIA=\"CSTM_KL_ENG_T_R5L_03C_\"$Panel_name\"_IRCVT00BF_KPD_LUCKY_JEDIA_20190417\"，其余三个类似\n",
                "\n",
                "#第一步建立生成目录\n",
                "\n",
                "rm -v -rf ${OUT_BIN_PATH}\n",
                "mkdir -v ${OUT_BIN_PATH}\n",
                "while [ \"$iBuildIndex2\" != \"$iConfigNumber\" ]\n",
                "do\n",
                "       Panel_name=${panel_name[$iBuildIndex2]}\n",
                "       OutBinPath=${OUT_BIN_PATH}/$Panel_name\n",
                "       JEDIA=\"",textBox9.Text,"\"$Panel_name\"",textBox8.Text,"\"\n",
                "       MIRROR_JEDIA=\"",textBox9.Text,"\"$Panel_name\"",textBox7.Text,"\"\n",
                "       MIRROR_VESA=\"",textBox9.Text,"\"$Panel_name\"",textBox6.Text,"\"\n",
                "       VESA=\"",textBox9.Text,"\"$Panel_name\"",textBox10.Text,"\"\n",
                "\n",
                "       mkdir -v $OutBinPath\n",
                "       mkdir -v $OutBinPath/$JEDIA\n",
                "       mkdir -v $OutBinPath/$MIRROR_JEDIA\n",
                "       mkdir -v $OutBinPath/$MIRROR_VESA\n",
                "       mkdir -v $OutBinPath/$VESA\n",
                "       iBuildIndex2=$(($iBuildIndex2+1))\n",
                "done\n\n",
                "chmod -R 777 ${OUT_BIN_PATH}\n",
                "\n",
                "while [ \"$iBuildIndex\" != \"$iConfigNumber\" ]\n",
                "do\n",
                "    echo \"-----------------------------------------\"\n",
                "    echo \"[iBuildIndex=$iBuildIndex]\"\n",
                "\n",
                "    #第二步，查看customer是4:3还是16:9,\n",
                "    # -lt:<      -gt:>       -ge:>=      -le:<=     -eq :==\n",
                "    cd ~/$WORK_PATH/project/boarddef\n",
                "    pwd\n",
                "\n",
                "    #在开始编译16：9的customer之前，将customer设置为16：9的customer\n",
                "    if [ $iBuildIndex -eq $i4_3Index ]\n",
                "    then\n",
                "	     sed -i \"s!$C4_3COSTOMER!$C16_9COSTOMER!g\" CustomSpec.h\n",
                "	     CURRENT_CUSTOMER=$C16_9COSTOMER_1\n",
                "    fi\n",
                "\n",
                "    #第三步修改屏参以及EDID，需要进入到customer\n",
                "    Panel_name=${panel_name[$iBuildIndex]}\n",
                "    EDID_UP=${EDID[$iBuildIndex]}\n",
                "    OutBinPath=${OUT_BIN_PATH}/$Panel_name\n",
                "    cd ~/$WORK_PATH/project/boarddef/$CUSTOMER_DIR\n",
                "    pwd\n",
                "\n",
               "        if [ $iBuildIndex -ge 0 ]\n",
               "        then\n",
               "                echo \"修改CUSTOMER\"\n",
               "                Panel_name_1=$PANEL_SELECT$Panel_name\n",
               "                EDID_UP_1=$EDID_SELECT$EDID_UP\n",
               "\n",
               "                sed -i \"/PANEL_DEFAULT_TYPE_SEL/c$Panel_name_1\" $CURRENT_CUSTOMER.h\n",
               "                sed -i \"/PANEL_EDID_TYPE_SEL/c$EDID_UP_1\" $CURRENT_CUSTOMER.h\n",
               "        fi\n",
               "        #第四步确定屏参,按照如下顺序编写正屏_JEIDA,倒屏_JEDIA,倒屏_VESA,正屏_VESA\n",
               "        #并且要把编好的软件放到对应的文件夹\n",
               "        # Get panel name\n",
               "\n",
               "        echo \"Panel_name=$Panel_name\"\n",
               "        iIndex=0\n",
               "        PANEL_INVERT_0=\"#define ENABLE_MIRROR_DEFAULT				  	0		//0:positive  1:inverted\"\n",
               "        PANEL_INVERT_1=\"#define ENABLE_MIRROR_DEFAULT				  	1		//0:positive  1:inverted\"\n",
               "        PANEL_LVDS_TYPE_0=\"#define PANEL_TI_MODE               			0		//0:JEDIA   1:VESA\"\n",
               "        PANEL_LVDS_TYPE_2=\"#define PANEL_TI_MODE               			1		//0:JEDIA   1:VESA\"\n",
               "#需要改七*\n",
               "#可以直接复制‘需要改六’的JEIDA...   此处的作用的把编好的软件放到对应文件夹\n",
               "    JEDIA=\"",textBox9.Text,"\"$Panel_name\"",textBox8.Text,"\"\n",
               "    MIRROR_JEDIA=\"",textBox9.Text,"\"$Panel_name\"",textBox7.Text,"\"\n",
               "    MIRROR_VESA=\"",textBox9.Text,"\"$Panel_name\"",textBox6.Text,"\"\n",
               "    VESA=\"",textBox9.Text,"\"$Panel_name\"",textBox10.Text,"\"\n",
               "    while [ $iIndex -le 4 ]\n",
               "    do\n",
               "                cd ~/$WORK_PATH/project/boarddef/$CUSTOMER_DIR\n",
               "                pwd\n",
               "\n",
               "    case ${iIndex} in\n",
               "        0)\n",
               "           echo 'This is 正屏_JEDIA'\n",
               "           sed -i \"/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_0\"  $CURRENT_CUSTOMER.h\n",
               "           sed -i \"/PANEL_TI_MODE/c$PANEL_LVDS_TYPE_0\"  $CURRENT_CUSTOMER.h\n",
               "           cd ~/$WORK_PATH\n",
               "           make clean ; make\n",
               "           cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$JEDIA/  ||  error_exit \"cannot copy *.bin file to directory\"\n",
               "       ;;\n",
               "       1)\n",
               "            echo 'This is 倒屏_JEDIA'\n",
               "            sed -i \"/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_1\" $CURRENT_CUSTOMER.h\n",
               "            cd ~/$WORK_PATH\n",
               "            make\n",
               "            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$MIRROR_JEDIA/ || error_exit \"cannot copy *.bin file to directory\"\n",
               "        ;;\n",
               "        2)\n",
               "            echo 'This is 倒屏_VESA'\n",
               "            sed -i \"/PANEL_TI_MODE/c$PANEL_LVDS_TYPE_2\" $CURRENT_CUSTOMER.h\n",
               "            cd ~/$WORK_PATH\n",
               "            make\n",
               "            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$MIRROR_VESA/ || error_exit \"cannot copy *.bin file to directory\"\n",
               "        ;;\n",
               "        3)\n",
               "            echo 'This is 正屏_VESA'\n",
               "            sed -i \"/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_0\" $CURRENT_CUSTOMER.h\n",
               "            cd ~/$WORK_PATH\n",
               "            make\n",
               "            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$VESA/ || error_exit \"cannot copy *.bin file to directory\"\n",
               "        ;;\n",
               "        *)\n",
               "            echo '编译完成'\n",
               "        ;;\n",
               "    esac\n",
               "    iIndex=$(($iIndex+1))\n",
               "    done\n",
               "\n",
               "    iBuildIndex=$(($iBuildIndex+1))\n",
               "done\n",
               "\n",
               "#cd configs/melody",
            };
            using (StreamWriter sw = new StreamWriter("v53_make_all_bin.sh"))
            {
                List<String> list = new List<String>();
                int a = 0;
                foreach (string s in names)
                {
                    list.Add(s);

                }
                richTextBox1.Text = richTextBox1.Text.TrimEnd((char[])"\r\n".ToCharArray());
                //richTextBox1.Text = Regex.Replace(richTextBox1.Text, @"\n...", "\n");

                foreach (String s in richTextBox1.Lines)
                {
                    list.Add("# " + s + "\n");
                    list.Add("panel_name[" + a + "]=" + s + "\n");
                    //查找本行的字符串中有没有_，没有就跳过
                    int x = s.IndexOf("_");
                    if (x == -1) continue;
                    //将本行字符串分隔成2个，以_分隔
                    //判断字符串中是否含有_有的话就分隔，直到没有为止
                    string[] sArray = new string[100];
                    if (s.IndexOf("_") > -1)
                    {
                        sArray = s.Split(new char[] { '_' }, 2);
                    }

                    //int y = sArray[1].ToString().IndexOf("_");
                    //if (y == -1) continue;

                    while (sArray[1].ToString().IndexOf("_") > -1)
                    {
                        sArray = sArray[1].ToString().Split(new char[] { '_' }, 2);
                    }
                    if (s != "\0")
                        list.Add("EDID[" + a + "]=EDID_VGA_HDMI_DEFAULT_" + sArray[1].ToUpper().ToString() + "\n\n");
                    a++;
                }

                label10.Text = a.ToString();

                foreach (string s in names1)
                {
                    list.Add(s);

                }
                richTextBox2.Text = richTextBox2.Text.TrimEnd((char[])"\r\n".ToCharArray());
                //richTextBox2.Text = Regex.Replace(richTextBox2.Text, @"\n...", "\n");

                foreach (String s in richTextBox2.Lines)
                {
                    if (s == "\n") continue;
                    list.Add("# " + s + "\n");
                    list.Add("panel_name[" + a + "]=" + s + "\n");

                    //查找本行的字符串中有没有_，没有就跳过
                    int x = s.IndexOf("_");
                    if (x == -1) continue;
                    //将本行字符串分隔成2个，以_分隔
                    //判断字符串中是否含有 '_' , 有的话就分隔，直到没有为止
                    string[] sArray = new string[100];

                    if (s.IndexOf("_") > -1)
                    {
                        sArray = s.Split(new char[] { '_' }, 2);
                    }

                    //int y = sArray[1].ToString().IndexOf("_");
                    //if (y == -1) continue;

                    while (sArray[1].ToString().IndexOf("_") > -1)
                    {
                        sArray = sArray[1].ToString().Split(new char[] { '_' }, 2);
                    }
                    if (s != "\0")
                        list.Add("EDID[" + a + "]=EDID_VGA_HDMI_DEFAULT_" + sArray[1].ToUpper().ToString() + "\n\n");
                    a++;
                }

                label9.Text = a.ToString();

                foreach (string s in names2)
                {
                    list.Add(s);
                }
                foreach (string s in list)
                {
                    sw.Write(s);
                }
            }
        }
        public String Get_Total_Panel_Number()
        {
            int a = 0;
            foreach (String s in richTextBox1.Lines)
            {
                a++;
            }
            foreach (String s in richTextBox2.Lines)
            {
                a++;
            }
            label9.Text = a.ToString();
            return label9.Text;
        }
        public String Get_4_3_Panel_Number()
        {
            int a = 0;
            foreach (String s in richTextBox1.Lines)
            {
                a++;
            }
            label10.Text = a.ToString();
            return label10.Text;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"
                        该工具的作用仅仅是为了生成脚本文件，
                        并不能直接编译软件，请知悉
                        在利用工具生成脚本之后，按照下面的步骤设置好，
                        然后将脚本文件放到代码的根目录下
                        ./v53_make_all_bin.sh    运行即可

                        在编译之前要确定的东西
                        首先要制定好customer，logo，IR                       

                        WORK_PATH：是编译代码的根目录
                        OUT_BIN_DIR:  这个是生成目录的文件夹名
                        CUSTOMER_DIR:  客户customer目录
                        CUSTOMER_4_3: 要编译的4_3的customer
                        CUSTOMER_16_9: 要编译的4_3的customer
                        PANEL_4_3: 要编译的4_3的屏参
                        PANEL_16_9: 要编译的16_9的屏参
                        全部填好后，点击GENERATE生成
                    ");
        }


    }
}
