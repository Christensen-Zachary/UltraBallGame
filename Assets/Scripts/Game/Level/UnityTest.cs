




using System.Buffers;

public static class UnityTest
{
	public static readonly string LEVEL0 = "12'0:0,4,250;0,5,100;0,6,100;0,7,100;0,8,100;0,9,100;0,10,100;0,11,100;0,12,100;0,13,100;0,14,100;1,14,250;2,14,250;3,14,250;4,14,250;5,14,250;6,14,250;7,14,250;8,14,250;9,14,250;10,14,250;11,14,100;11,13,100;11,12,100;11,11,100;11,10,100;11,9,100;11,8,100;11,7,100;11,6,100;11,5,100;11,4,250;10,4,250;9,4,250;8,4,250;7,4,250;6,4,250;5,4,250;4,4,250;3,4,250;1,4,250;2,6,250;2,7,250;2,8,250;2,9,250;2,10,250;2,11,250;2,12,250;3,12,250;4,12,250;5,12,250;6,12,250;7,12,250;8,12,250;9,12,250;9,11,250;9,10,250;9,9,250;9,8,250;9,7,250;9,6,250;8,6,250;7,6,250;6,6,250;5,6,250;4,6,250;4,8,250;4,9,250;4,10,250;5,10,250;6,10,250;7,10,250;7,9,250;7,8,250;6,8,250;1,16,350;2,16,350;3,16,350;4,16,350;5,16,350;6,16,350;7,16,350;8,16,350;9,16,350;10,16,350;~11:3,6,0;~6:2,4,0;10,20,350;9,20,350;8,20,350;7,20,350;6,20,350;5,20,350;4,20,350;3,20,350;2,20,350;1,20,350;1,21,350;2,21,350;3,21,350;4,21,350;5,21,350;6,21,350;7,21,350;8,21,350;9,21,350;10,21,350;~";
	public static readonly string LEVEL1 = "12'0:5,12,50;6,12,50;5,11,50;6,11,50;3,11,50;8,11,50;5,10,50;6,10,50;7,10,50;4,10,50;3,10,50;9,11,50;10,11,50;2,11,50;1,11,50;3,9,50;4,9,50;5,9,50;6,9,50;7,9,50;8,10,50;8,9,50;9,10,50;2,10,50;6,8,50;4,8,50;5,8,50;7,8,50;1,6,100;2,6,100;3,6,100;4,6,100;5,6,100;6,6,100;7,6,100;8,6,100;9,6,100;10,6,100;0,15,100;1,15,100;2,15,100;3,15,100;4,15,100;7,15,100;8,15,100;9,15,100;10,15,100;11,15,100;~2:6,13,50;1,10,50;2,9,50;7,11,50;~1:5,13,50;10,10,50;9,9,50;4,11,50;~4:11,11,50;6,7,50;~3:0,11,50;5,7,50;~7:0,2,100;~8:11,2,100;~5:7,2,100;6,2,100;5,2,100;4,2,100;~11:5,15,0;6,15,0;~6:0,10,0;11,10,0;~";
	// public static readonly string LEVEL1 = "12'0:5,7,70;3,7,70;7,7,70;7,8,70;6,8,70;5,8,70;4,8,70;3,8,70;8,9,70;7,9,70;6,9,70;4,9,70;3,9,70;2,9,70;1,10,70;5,10,70;9,10,70;8,10,70;2,10,70;3,10,70;1,11,70;7,10,70;1,12,70;2,12,70;3,12,70;4,12,70;5,12,70;5,11,70;6,12,70;7,12,70;8,12,70;9,12,70;9,11,70;9,13,70;8,13,70;7,13,70;6,13,70;5,13,70;4,13,70;3,13,70;2,13,70;1,13,70;2,14,70;3,14,70;4,14,70;5,14,70;6,14,70;7,14,70;8,14,70;7,15,70;6,15,70;5,15,70;4,15,70;3,15,70;2,15,1;1,15,1;0,15,1;1,14,1;0,14,1;0,13,1;0,12,1;0,11,1;0,10,1;0,9,1;0,8,1;0,7,1;0,6,1;1,6,1;1,7,1;1,8,1;1,9,1;2,8,1;2,7,1;2,6,1;3,6,1;4,7,1;4,6,1;5,6,1;6,7,1;6,6,1;7,6,1;8,8,1;8,7,1;8,6,1;9,9,1;9,8,1;9,7,1;9,6,1;10,6,1;10,7,1;10,8,1;10,9,1;10,10,1;10,11,1;10,12,1;10,13,1;10,14,1;9,14,1;9,15,1;8,15,1;10,15,1;11,15,1;11,14,1;11,13,1;11,12,1;11,11,1;11,10,1;11,9,1;11,8,1;11,7,1;11,6,1;6,10,1;6,11,1;7,11,1;8,11,1;2,11,1;3,11,1;4,11,1;4,10,1;5,9,1;~5:0,5,1;11,5,1;5,5,1;3,5,1;7,5,1;~";
	public static readonly string LEVEL2 = "12'0:2,5,200;3,5,200;4,5,200;5,5,200;6,5,200;7,5,200;8,5,200;9,5,200;10,5,200;10,6,200;10,7,200;10,8,200;10,9,200;10,10,200;10,11,200;10,12,200;10,13,200;10,14,200;10,15,200;9,15,200;8,15,200;7,15,200;6,15,200;5,15,200;4,15,200;3,15,200;2,15,200;1,15,200;1,14,200;1,13,200;1,12,200;1,11,200;1,10,200;1,9,200;1,8,200;1,7,200;2,7,200;3,7,200;4,7,200;5,7,200;6,7,200;7,7,200;8,7,200;8,8,200;8,9,200;8,10,200;8,11,200;8,12,200;8,13,200;7,13,200;6,13,200;5,13,200;4,13,200;3,13,200;3,12,200;3,11,200;3,10,200;3,9,200;4,9,200;5,9,200;6,9,200;6,10,200;6,11,200;5,11,200;~";
	public static readonly string LEVEL3 = "12'0:1,15,200;3,15,200;5,15,200;7,15,200;9,15,200;11,15,200;11,13,50;1,13,50;3,13,50;5,13,50;7,13,50;9,13,50;11,11,50;1,11,50;3,11,50;5,11,50;7,11,50;9,11,50;11,9,50;1,9,50;3,9,50;5,9,50;7,9,50;9,9,50;11,7,50;1,7,50;3,7,50;5,7,50;7,7,50;9,7,50;11,5,50;1,5,50;3,5,50;5,5,50;7,5,50;9,5,50;2,6,50;4,6,50;6,6,50;8,6,50;10,6,50;2,8,50;4,8,50;6,8,50;8,8,50;10,8,50;2,10,50;4,10,50;6,10,50;8,10,50;10,10,50;2,12,50;4,12,50;6,12,50;8,12,50;10,12,50;2,14,50;4,14,50;6,14,50;8,14,50;10,14,50;1,17,200;3,17,200;5,17,200;7,17,200;9,17,200;11,17,200;10,16,200;8,16,200;6,16,200;2,16,200;4,16,200;1,19,200;3,19,200;5,19,200;7,19,200;9,19,200;11,19,200;10,18,200;8,18,200;6,18,200;2,18,200;4,18,200;~";
	public static readonly string LEVEL4 = "12'0:2,4,45;4,4,45;6,4,45;8,4,45;2,5,45;3,5,45;4,5,45;5,5,45;6,5,45;7,5,45;8,5,45;8,6,45;7,6,45;3,6,45;2,6,45;1,6,45;9,6,45;9,7,45;8,7,45;7,7,45;3,7,45;2,7,45;1,7,45;0,7,45;0,8,45;1,8,45;2,8,45;3,8,45;4,8,45;5,8,45;6,8,45;7,8,45;8,8,45;9,8,45;0,9,45;0,10,45;0,11,45;10,11,45;10,10,45;10,9,45;10,8,45;10,7,45;3,14,45;4,14,45;5,14,45;6,14,45;7,14,45;2,13,45;3,13,45;4,13,45;5,13,45;6,13,45;7,13,45;8,13,45;1,12,45;2,12,45;3,12,45;4,12,45;5,12,45;6,12,45;7,12,45;8,12,45;9,12,45;9,11,45;8,11,45;7,11,45;6,11,45;5,11,45;4,11,45;3,11,45;2,11,45;1,11,45;5,10,45;5,9,45;4,10,45;3,10,45;6,10,45;7,10,45;5,15,45;1,13,45;0,12,45;2,14,45;8,14,45;6,15,45;9,13,45;10,12,45;4,15,45;6,7,45;4,7,45;~3:6,6,45;~4:4,6,45;~";
	//public static readonly string LEVEL3 = "12'0:1,15,10;3,15,10;5,15,10;7,15,10;9,15,10;11,15,10;11,13,10;1,13,10;3,13,10;5,13,10;7,13,10;9,13,10;11,11,10;1,11,10;3,11,10;5,11,10;7,11,10;9,11,10;11,9,12;1,9,12;3,9,12;5,9,12;7,9,12;9,9,12;11,7,16;1,7,16;3,7,16;5,7,16;7,7,16;9,7,16;11,5,20;1,5,20;3,5,20;5,5,20;7,5,20;9,5,20;2,6,18;4,6,18;6,6,18;8,6,18;10,6,18;2,8,14;4,8,14;6,8,14;8,8,14;10,8,14;2,10,10;4,10,10;6,10,10;8,10,10;10,10,10;2,12,10;4,12,10;6,12,10;8,12,10;10,12,10;2,14,10;4,14,10;6,14,10;8,14,10;10,14,10;1,17,10;3,17,10;5,17,10;7,17,10;9,17,10;11,17,10;10,16,10;8,16,10;6,16,10;2,16,10;4,16,10;1,19,10;3,19,10;5,19,10;7,19,10;9,19,10;11,19,10;10,18,10;8,18,10;6,18,10;2,18,10;4,18,10;2,5,15;4,5,15;6,5,15;8,5,15;10,5,15;~";
	public static readonly string LEVEL5 = "12'0:5,4,70;4,4,70;3,4,70;2,4,35;1,4,35;0,4,70;0,5,35;1,5,35;2,5,70;3,5,70;4,5,70;5,5,70;5,7,70;0,7,70;1,7,70;2,7,35;3,7,35;4,7,70;5,6,70;4,6,70;3,6,70;2,6,35;1,6,35;0,6,70;5,9,35;0,9,70;1,9,70;2,9,70;3,9,70;4,9,35;5,8,70;4,8,35;3,8,35;2,8,70;1,8,70;0,8,70;5,11,70;0,11,70;1,11,70;2,11,35;3,11,35;4,11,70;5,10,70;4,10,35;3,10,35;2,10,70;1,10,70;0,10,70;5,13,70;0,13,35;1,13,35;2,13,70;3,13,70;4,13,70;5,12,70;4,12,70;3,12,70;2,12,35;1,12,35;0,12,70;7,13,70;8,13,70;9,13,70;10,13,35;11,13,35;11,12,70;10,12,35;9,12,35;8,12,70;7,12,70;7,11,70;8,11,35;9,11,35;10,11,70;11,11,70;11,10,70;11,9,70;11,8,70;11,7,35;11,6,70;11,5,70;11,4,70;10,4,70;10,5,70;10,6,35;10,7,35;10,8,35;10,9,70;10,10,70;9,10,70;9,9,35;9,8,35;9,7,70;9,6,35;9,5,35;9,4,70;8,4,35;8,5,35;8,6,70;8,7,70;8,8,70;8,9,35;8,10,35;7,10,35;7,9,70;7,8,70;7,7,70;7,6,70;7,5,70;7,4,35;0,15,250;2,15,250;4,15,250;6,15,250;8,15,250;10,15,250;~11:6,4,0;6,5,0;6,6,0;6,7,0;6,8,0;6,9,0;6,10,0;6,11,0;6,12,0;6,13,0;~";
	public static readonly string LEVEL6 = "12'0:0,15,50;1,15,50;0,14,50;1,14,50;0,13,50;0,10,50;1,11,50;2,12,50;3,12,50;4,11,50;5,10,50;6,9,50;7,8,50;7,7,50;6,7,50;6,8,50;5,7,50;5,6,50;4,6,50;3,6,50;2,6,50;1,6,50;0,6,50;0,7,50;0,8,50;0,9,50;1,9,50;1,10,50;1,8,50;1,7,50;2,7,50;3,7,50;4,7,50;5,8,50;5,9,50;4,10,50;3,10,50;2,10,50;2,11,50;3,11,50;2,9,50;2,8,50;4,15,50;5,15,50;6,15,50;8,15,50;9,15,50;10,15,50;11,15,50;11,14,50;11,13,50;11,12,50;10,11,50;9,10,50;8,10,50;7,11,50;7,12,50;6,13,50;5,13,50;4,14,50;5,14,50;6,14,50;7,14,50;8,14,50;7,13,50;8,13,50;8,12,50;8,11,50;9,11,50;9,12,50;10,12,50;8,6,50;8,5,50;9,5,50;9,6,50;10,5,50;11,5,50;10,6,50;11,6,50;11,7,50;10,7,50;10,8,50;11,8,50;11,9,50;~6:3,9,50;4,9,50;4,8,50;3,8,50;9,13,50;9,14,50;10,14,50;10,13,50;~";
	public static readonly string LEVEL7 = "12'0:0,16,64;0,17,74;0,18,73;0,19,83;0,20,1;1,16,1;1,17,54;1,18,60;1,19,84;1,20,61;2,16,1;2,18,61;2,19,76;2,20,58;2,22,1;3,16,53;3,18,1;3,19,65;3,20,1;3,22,1;4,16,54;4,17,1;4,18,55;4,19,1;4,20,1;4,21,1;4,22,62;5,16,1;5,17,1;5,18,1;5,19,1;5,20,1;5,21,1;5,22,1;6,16,59;6,17,76;6,18,1;6,19,1;6,20,1;6,22,1;7,16,68;7,17,77;7,18,60;7,19,50;7,20,68;7,21,1;7,22,1;8,16,57;8,17,68;8,18,81;8,19,82;8,20,63;8,21,84;8,22,1;9,16,1;9,17,1;9,18,70;9,19,53;9,20,70;9,21,70;9,22,1;10,16,1;10,18,1;10,19,65;10,20,76;10,21,67;10,22,1;11,16,1;11,17,55;11,18,1;11,19,68;11,20,68;11,21,1;11,22,1;0,5,61;0,6,51;0,7,51;0,8,1;0,9,67;0,10,89;0,11,65;0,12,56;0,13,54;0,14,82;0,15,71;1,5,1;1,6,1;1,7,1;1,8,65;1,9,1;1,10,1;1,11,50;1,12,1;1,13,1;1,14,1;1,15,1;2,7,1;2,8,76;2,9,1;2,10,1;2,11,1;2,12,1;2,13,1;2,15,1;3,5,1;3,6,1;3,7,1;3,8,56;3,9,55;3,10,64;3,11,1;3,12,1;3,13,1;3,14,51;3,15,50;4,5,56;4,6,63;4,7,81;4,8,91;4,9,89;4,10,95;4,11,52;4,12,1;4,13,60;4,14,53;4,15,1;5,5,65;5,6,1;5,7,1;5,8,61;5,9,72;5,10,50;5,11,1;5,12,1;5,13,1;5,14,1;5,15,1;6,5,64;6,6,1;6,7,1;6,8,1;6,9,1;6,11,1;6,13,1;6,14,50;6,15,1;7,5,61;7,6,1;7,7,56;7,8,1;7,9,1;7,10,1;7,11,1;7,12,1;7,13,67;7,14,1;7,15,61;8,5,78;8,6,67;8,7,1;8,9,1;8,10,1;8,11,1;8,12,77;8,13,54;8,14,62;8,15,55;9,5,57;9,6,1;9,7,1;9,8,1;9,9,1;9,10,62;9,11,82;9,12,77;9,13,1;9,14,1;9,15,56;10,7,52;10,8,56;10,9,54;10,10,99;10,11,80;10,12,77;10,13,61;10,14,1;10,15,58;11,5,55;11,6,1;11,7,1;11,8,57;11,9,78;11,10,93;11,11,54;11,12,68;11,13,77;11,14,57;11,15,65;~";
	public static readonly string LEVEL8 = "12'0:1,4,200;1,5,200;1,6,200;1,7,200;1,8,200;1,9,200;1,10,200;1,11,200;1,12,200;1,13,200;1,14,200;1,15,200;3,15,200;3,14,200;3,13,200;3,12,200;3,11,200;3,10,200;3,9,200;3,8,200;3,7,200;3,6,200;3,5,200;3,4,200;5,15,200;5,14,200;5,13,200;5,12,200;5,11,200;5,10,200;5,9,200;5,8,200;5,7,200;5,6,200;5,5,200;5,4,200;7,15,200;7,14,200;7,13,200;7,12,200;7,11,200;7,10,200;7,9,200;7,8,200;7,7,200;7,6,200;7,5,200;7,4,200;9,15,200;9,14,200;9,13,200;9,12,200;9,11,200;9,10,200;9,9,200;9,8,200;9,7,200;9,6,200;9,5,200;9,4,200;11,15,200;11,14,200;11,13,200;11,12,200;11,11,200;11,10,200;11,9,200;11,8,200;11,7,200;11,6,200;11,5,200;11,4,200;~";
	public static readonly string LEVEL9 = "12'0:4,15,300;4,14,300;4,13,300;4,12,300;4,11,300;4,10,300;3,9,300;2,8,300;1,7,300;1,6,300;2,5,300;3,4,300;4,4,300;5,4,300;6,4,300;7,4,300;8,4,300;9,5,300;10,6,300;10,7,300;9,8,300;8,9,300;7,10,300;7,11,300;7,12,300;7,13,300;7,14,300;7,15,300;10,8,300;9,9,300;10,9,300;10,10,300;10,11,300;~3:9,11,300;~4:11,11,300;~6:11,12,300;10,12,300;9,12,300;~";
	public static readonly string LEVEL10 = "12'13:0,5,10;0,6,10;0,7,10;0,8,10;0,9,10;0,10,10;0,11,10;0,12,10;0,13,10;0,14,10;0,15,10;~0:1,5,70;1,6,70;1,7,70;1,8,70;1,9,70;1,10,70;1,11,70;1,12,70;1,13,70;2,13,70;2,12,70;2,11,70;2,10,70;2,9,70;2,8,70;2,7,70;2,6,70;2,5,70;3,5,70;3,6,70;3,7,70;3,8,70;3,9,70;3,10,70;3,11,70;3,12,70;3,13,70;4,13,70;4,12,70;4,11,70;4,10,70;4,9,70;4,8,70;4,7,70;4,6,70;4,5,70;5,13,70;5,12,70;5,11,70;5,10,70;5,9,70;5,8,70;5,7,70;5,6,70;5,5,70;6,13,70;6,12,70;6,11,70;6,10,70;6,9,70;6,8,70;6,7,70;6,6,70;6,5,70;7,13,70;7,12,70;7,11,70;7,10,70;7,9,70;7,8,70;7,7,70;7,6,70;7,5,70;8,13,70;8,12,70;8,11,70;8,10,70;8,9,70;8,8,70;8,7,70;8,6,70;8,5,70;9,13,70;9,12,70;9,11,70;9,10,70;9,9,70;9,8,70;9,7,70;9,6,70;9,5,70;10,13,70;10,12,70;10,11,70;10,10,70;10,9,70;10,8,70;10,7,70;10,6,70;10,5,70;11,13,70;11,12,70;11,11,70;11,10,70;11,9,70;11,8,70;11,7,70;11,6,70;11,5,70;1,14,70;2,14,70;3,14,70;4,14,70;5,14,70;6,14,70;7,14,70;8,14,70;9,14,70;10,14,70;11,14,70;1,15,70;2,15,70;3,15,70;4,15,70;5,15,70;6,15,70;7,15,70;8,15,70;9,15,70;10,15,70;11,15,70;~";
	public static readonly string LEVEL11 = "12'0:1,4,1;2,4,1;3,4,1;4,4,1;5,4,1;6,4,1;7,4,1;8,4,1;9,4,1;10,4,1;10,5,1;1,5,1;2,5,1;3,5,1;4,5,1;5,5,1;6,5,1;7,5,1;8,5,1;9,5,1;10,6,1;1,6,1;2,6,1;3,6,1;4,6,1;5,6,1;6,6,1;7,6,1;8,6,1;9,6,1;10,7,1;1,7,1;2,7,1;3,7,1;4,7,1;5,7,1;6,7,1;7,7,1;8,7,1;9,7,1;10,8,1;1,8,1;2,8,1;3,8,1;4,8,1;5,8,1;6,8,1;7,8,1;8,8,1;9,8,1;10,9,1;1,9,1;2,9,1;3,9,1;4,9,1;5,9,1;6,9,1;7,9,1;8,9,1;9,9,1;10,10,1;1,10,1;2,10,1;3,10,1;4,10,1;5,10,1;6,10,1;7,10,1;8,10,1;9,10,1;10,11,1;1,11,1;2,11,1;3,11,1;4,11,1;5,11,1;6,11,1;7,11,1;8,11,1;9,11,1;10,12,1;1,12,1;2,12,1;3,12,1;4,12,1;5,12,1;6,12,1;7,12,1;8,12,1;9,12,1;10,13,1;1,13,1;2,13,1;3,13,1;4,13,1;5,13,1;6,13,1;7,13,1;8,13,1;9,13,1;10,14,1;1,14,1;2,14,1;3,14,1;4,14,1;5,14,1;6,14,1;7,14,1;8,14,1;9,14,1;10,15,1;1,15,1;2,15,1;3,15,1;4,15,1;5,15,1;6,15,1;7,15,1;8,15,1;9,15,1;~";
	public static readonly string LEVEL12 = "12'0:4,3,500;7,3,500;8,3,500;9,3,500;10,3,500;10,4,500;10,5,500;10,6,500;10,7,500;10,8,500;10,9,500;10,10,500;10,11,500;10,12,500;10,13,500;10,14,500;10,15,500;9,15,500;8,15,500;7,15,500;6,15,500;5,15,500;4,15,500;3,15,500;2,15,500;1,15,500;1,14,500;1,13,500;1,12,500;1,11,500;1,10,500;1,9,500;1,8,500;1,7,500;1,6,500;1,5,500;1,4,500;1,3,500;2,3,500;3,3,500;3,5,500;4,5,500;5,5,500;6,5,500;7,5,500;8,5,500;8,6,500;8,7,500;8,8,500;8,9,500;8,10,500;8,11,500;8,12,500;8,13,500;7,13,500;6,13,500;5,13,500;4,13,500;3,13,500;3,12,500;3,11,500;3,10,500;3,9,500;3,8,500;3,7,500;3,6,500;5,11,750;6,11,750;6,10,750;5,10,750;5,9,750;5,8,750;5,7,750;6,7,750;6,8,750;6,9,750;~8:6,2,0;~11:2,5,0;9,5,0;9,13,180;2,13,180;~6:2,12,0;2,11,0;2,10,0;2,9,0;2,8,0;2,7,0;2,6,0;9,6,0;9,7,0;9,8,0;9,9,0;9,10,0;9,11,0;9,12,0;~13:4,12,0;5,12,0;6,12,0;7,12,0;7,11,0;7,10,0;7,9,0;7,8,0;7,7,0;7,6,0;6,6,0;5,6,0;4,6,0;4,7,0;4,8,0;4,9,0;4,10,0;4,11,0;~";
	public static readonly string LEVEL13 = "9'0:0,4,100;1,4,100;2,4,100;3,4,100;4,4,100;5,4,100;6,4,100;7,4,100;5,6,100;4,7,100;2,9,100;1,10,100;1,7,100;1,6,100;5,9,100;5,10,100;7,10,100;7,9,100;6,8,100;~1:7,6,100;~6:8,4,0;~";

	public static readonly string LEVEL696969   = "12'0:1,5,10;1,4,10;1,6,10;1,7,10;3,7,10;3,6,10;3,5,10;6,7,10;6,6,10;6,5,10;6,4,10;8,7,10;8,6,10;8,5,10;8,8,10;7,8,10;6,8,10;5,8,10;4,8,10;3,8,10;2,8,10;1,8,10;1,9,10;1,10,10;0,11,10;0,12,10;0,13,10;1,14,10;2,14,10;2,9,10;3,9,10;4,9,10;5,9,10;6,9,10;7,9,10;8,9,10;8,10,10;8,11,10;9,11,10;9,12,10;8,12,10;10,12,10;10,11,10;9,10,10;10,10,10;3,10,10;4,10,10;5,10,10;6,10,10;~1:8,13,10;10,13,10;7,10,10;1,11,10;3,15,10;~2:2,10,10;0,14,10;~6:0,9,10;~4:3,14,10;~13:9,13,10;~";

}