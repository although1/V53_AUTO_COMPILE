#export CC=/usr/bin/gcc
#在编译之前要确定的东西，首先要制定好customer，logo，IR
#最后本脚本一共有七处需要手动修改，有些可以不改，加了*的是必改
#在编译之前要先生成存放软件的目录，请先看‘需要改一’

#需要改一 
#OUT_BIN_PATH：这个是生成在根目录的文件夹，里面存放软件，可以不改
OUT_BIN_PATH=./YDG_SOFTWARE
OUT_BIN_DIR=Bin_R2_MELODY_ATV_MM_32MB_CUS_SZ

#需要改二 *
#OUT_BIN_NAME：这个是生成软件的文件名
#WORK_PATH：是你代码的根目录
OUT_BIN_NAME=YDG.V53.bin
WORK_PATH=mstar_v53
CUSTOMER_DIR=DC

#需要改三 *
#以下四个是customize.h 中你建立好的customer，用于切换customize.h中的4:3,16:9
CUSTOMER_SELECT="#define CUSTOMER_SELECT    "
C4_3COSTOMER_1=CSTM_DC_T_V53_03C_DC03C0001_4_3_32M_RD_DEFAULT
C4_3COSTOMER=$CUSTOMER_SELECT$C4_3COSTOMER_1

C16_9COSTOMER_1=CSTM_DC_T_V53_03C_DC03C0002_16_9_32M_RD_DEFAULT
C16_9COSTOMER=$CUSTOMER_SELECT$C16_9COSTOMER_1

CURRENT_CUSTOMER=$C4_3COSTOMER_1

#需要改四 *
#iConfigNumber：是最大屏参数，若当前最大为14，则填15
iConfigNumber=10

#需要改五 *

#i4_3Index：是16:9的第一个屏参的编号，用于切换customize.h中的4:3,16:9
i4_3Index=4

########################
#
# Generate Panel data
#
########################
#############################################   4:3   #########################################

#注意panel_name中的x按照屏参名来写

PANEL_SELECT="#define PANEL_DEFAULT_TYPE_SEL					"
EDID_SELECT="#define PANEL_EDID_TYPE_SEL                   	"

# PNL_LTM150X0_L01_1024X768
panel_name[0]=PNL_LTM150X0_L01_1024X768
EDID[0]=EDID_VGA_HDMI_DEFAULT_1024X768

# PNL_NL10276AC30_04_XGA_1024X768
panel_name[1]=PNL_NL10276AC30_04_XGA_1024X768
EDID[1]=EDID_VGA_HDMI_DEFAULT_1024X768

# PNL_LTM190EP01_SXGA_1280X1024
panel_name[2]=PNL_LTM190EP01_SXGA_1280X1024
EDID[2]=EDID_VGA_HDMI_DEFAULT_1280X1024

# PNL_LQ150U1LA03_UXGA_1600X1200
panel_name[3]=PNL_LQ150U1LA03_UXGA_1600X1200
EDID[3]=EDID_VGA_HDMI_DEFAULT_1600X1200

#############################################   16:9   #########################################

# PNL_V296W1_L14_WXGA_1280X768
panel_name[4]=PNL_V296W1_L14_WXGA_1280X768
EDID[4]=EDID_VGA_HDMI_DEFAULT_1280X768

# PNL_CLAA154WA05AN_WVGA_1280X800
panel_name[5]=PNL_CLAA154WA05AN_WVGA_1280X800
EDID[5]=EDID_VGA_HDMI_DEFAULT_1280X800

# PNL_SAM20_LTM200kT10_WXGA_1600X900
panel_name[6]=PNL_SAM20_LTM200kT10_WXGA_1600X900
EDID[6]=EDID_VGA_HDMI_DEFAULT_1600X900

# PNL_WSXGA_AU22_M201EW01_1680X1050
panel_name[7]=PNL_WSXGA_AU22_M201EW01_1680X1050
EDID[7]=EDID_VGA_HDMI_DEFAULT_1680X1050

# PNL_FULLHD_CMO216_H1L01_1920X1080
panel_name[8]=PNL_FULLHD_CMO216_H1L01_1920X1080
EDID[8]=EDID_VGA_HDMI_DEFAULT_1920X1080

# PNL_CLAA260WUA_WUXGA_1920X1200
panel_name[9]=PNL_CLAA260WUA_WUXGA_1920X1200
EDID[9]=EDID_VGA_HDMI_DEFAULT_1920X1200

#=======================================================
echo "===================================================="
echo "Start build YDG SOFTWARE "

function error_exit {
  echo "$1" 1>&2
  exit 1
}
#===================================================

iBuildIndex=0
iBuildIndex1=0
iBuildIndex2=0

echo "===================================================="
echo "Start build YDG SOFTWARE "
#需要改六 *
#需要改的地方是里面的JEDIA,MIRROR_JEDIA,MIRROR_VESA,VESA
#比如正常文件夹名为CSTM_KL_ENG_T_R5L_03C_05_GENERAL_1440x900_IRCVT00BF_KPD_LUCKY_JEDIA_20190417
#则要改成	JEDIA="CSTM_KL_ENG_T_R5L_03C_"$Panel_name"_IRCVT00BF_KPD_LUCKY_JEDIA_20190417"，其余三个类似

#设置交叉编译环境
export PATH=$PATH:/opt/tool/mips-4.3/bin
export PATH=$PATH:/opt/tool/aeon/bin
export PATH=$PATH:/opt/tool/arm/4.4.3/bin

#第一步建立生成目录

rm -v -rf ${OUT_BIN_PATH}
mkdir -v ${OUT_BIN_PATH}
while [ "$iBuildIndex2" != "$iConfigNumber" ]
do
       Panel_name=${panel_name[$iBuildIndex2]}
       OutBinPath=${OUT_BIN_PATH}/$Panel_name
       JEDIA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_JEDIA_20191209"
       MIRROR_JEDIA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_M_JEDIA_20191209"
       MIRROR_VESA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_M_VESA_20191209"
       VESA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_VESA_20191209"

       mkdir -v $OutBinPath
       mkdir -v $OutBinPath/$JEDIA
       mkdir -v $OutBinPath/$MIRROR_JEDIA
       mkdir -v $OutBinPath/$MIRROR_VESA
       mkdir -v $OutBinPath/$VESA
       iBuildIndex2=$(($iBuildIndex2+1))
done

chmod -R 777 ${OUT_BIN_PATH}

while [ "$iBuildIndex" != "$iConfigNumber" ]
do
    echo "-----------------------------------------"
    echo "[iBuildIndex=$iBuildIndex]"

    #第二步，查看customer是4:3还是16:9,
    # -lt:<      -gt:>       -ge:>=      -le:<=     -eq :==
    cd ~/$WORK_PATH/project/boarddef
    pwd

    #在开始编译16：9的customer之前，将customer设置为16：9的customer
    if [ $iBuildIndex -eq $i4_3Index ]
    then
	     sed -i "s!$C4_3COSTOMER!$C16_9COSTOMER!g" CustomSpec.h
	     CURRENT_CUSTOMER=$C16_9COSTOMER_1
    fi

    #第三步修改屏参以及EDID，需要进入到customer
    Panel_name=${panel_name[$iBuildIndex]}
    EDID_UP=${EDID[$iBuildIndex]}
    OutBinPath=${OUT_BIN_PATH}/$Panel_name
    cd ~/$WORK_PATH/project/boarddef/$CUSTOMER_DIR
    pwd

        if [ $iBuildIndex -ge 0 ]
        then
                echo "修改CUSTOMER"
                Panel_name_1=$PANEL_SELECT$Panel_name
                EDID_UP_1=$EDID_SELECT$EDID_UP

                sed -i "/PANEL_DEFAULT_TYPE_SEL/c$Panel_name_1" $CURRENT_CUSTOMER.h
                sed -i "/PANEL_EDID_TYPE_SEL/c$EDID_UP_1" $CURRENT_CUSTOMER.h
        fi
        #第四步确定屏参,按照如下顺序编写正屏_JEIDA,倒屏_JEDIA,倒屏_VESA,正屏_VESA
        #并且要把编好的软件放到对应的文件夹
        # Get panel name

        echo "Panel_name=$Panel_name"
        iIndex=0
        PANEL_INVERT_0="#define ENABLE_MIRROR_DEFAULT				  	0		//0:positive  1:inverted"
        PANEL_INVERT_1="#define ENABLE_MIRROR_DEFAULT				  	1		//0:positive  1:inverted"
        PANEL_LVDS_TYPE_0="#define PANEL_TI_MODE               			0		//0:JEDIA   1:VESA"
        PANEL_LVDS_TYPE_2="#define PANEL_TI_MODE               			1		//0:JEDIA   1:VESA"
#需要改七*
#可以直接复制‘需要改六’的JEIDA...   此处的作用的把编好的软件放到对应文件夹
    JEDIA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_JEDIA_20191209"
    MIRROR_JEDIA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_M_JEDIA_20191209"
    MIRROR_VESA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_M_VESA_20191209"
    VESA="CSTM_DC_CHINESE_TV5303C_"$Panel_name"_IRCVT00BF_08F7_KPD_LUCKY_LOGO_BLUE_VESA_20191209"
    while [ $iIndex -le 4 ]
    do
                cd ~/$WORK_PATH/project/boarddef/$CUSTOMER_DIR
                pwd

    case ${iIndex} in
        0)
           echo 'This is 正屏_JEDIA'
           sed -i "/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_0"  $CURRENT_CUSTOMER.h
           sed -i "/PANEL_TI_MODE/c$PANEL_LVDS_TYPE_0"  $CURRENT_CUSTOMER.h
           cd ~/$WORK_PATH
           make clean ; make
           cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$JEDIA/  ||  error_exit "cannot copy *.bin file to directory"
       ;;
       1)
            echo 'This is 倒屏_JEDIA'
            sed -i "/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_1" $CURRENT_CUSTOMER.h
            cd ~/$WORK_PATH
            make
            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$MIRROR_JEDIA/ || error_exit "cannot copy *.bin file to directory"
        ;;
        2)
            echo 'This is 倒屏_VESA'
            sed -i "/PANEL_TI_MODE/c$PANEL_LVDS_TYPE_2" $CURRENT_CUSTOMER.h
            cd ~/$WORK_PATH
            make
            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$MIRROR_VESA/ || error_exit "cannot copy *.bin file to directory"
        ;;
        3)
            echo 'This is 正屏_VESA'
            sed -i "/ENABLE_MIRROR_DEFAULT/c$PANEL_INVERT_0" $CURRENT_CUSTOMER.h
            cd ~/$WORK_PATH
            make
            cp -v  ./$OUT_BIN_DIR/$OUT_BIN_NAME  $OutBinPath/$VESA/ || error_exit "cannot copy *.bin file to directory"
        ;;
        *)
            echo '编译完成'
        ;;
    esac
    iIndex=$(($iIndex+1))
    done

    iBuildIndex=$(($iBuildIndex+1))
done

#cd configs/melody