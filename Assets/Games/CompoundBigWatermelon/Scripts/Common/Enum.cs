using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGP_004CompoundBigWatermelon
{
	/// <summary>
	/// 水果类型
	/// 注意：其中顺序是合成的顺序
	/// </summary>
	public enum FruitSeriesType
	{

		Mangosteen = 0,		// 山竹1
		Passionfruit = 1,	// 百香果
		Guava = 2,			// 番石榴
		Lemon = 3,			// 柠檬1
		Kiwi = 4,           // 猕猴桃1
		Pawpaw = 5,			// 木瓜
		Peach = 6,			// 桃子1
		Pineapple = 7,		// 菠萝1
		Coco = 8,           // 椰子1
		Watermelon = 9,		// 西瓜1
		BigWatermelon = 10,	// 大西瓜1

		SUM_COUNT = 11,		//总数（计数使用）
	}
}
