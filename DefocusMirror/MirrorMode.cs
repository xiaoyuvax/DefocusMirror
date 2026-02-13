namespace DefocusMirror
{
    public enum MirrorMode
    {
        Red,  //红色通道不模糊，模糊其他两个通道
        Blue, //蓝色通道不模糊，模糊其他两个通道
        Custom  //自定义模糊参数
    }
}