/// <summary>
/// QuizManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 제시 : 다음 빛의 구체와 문자를 기억하세요. (게임 시작과 동시에 제시)
/// 1) 빛의 구체 7가지 중 택1 
/// 2) 문자열 N가지 중 택1  (문자열은 계속 늘어날 예정) - '집중 평화 용서 감사 침착 정의 조화 자유 정직 지혜 친절 이해 활력 영감 공감 겸손 힘 지성 고요 결단 사랑 관용 자비 부드러움 축복 생명 믿음 젊음 자신감 덕성 행복 영혼 민첩성 건강 충만 일관성 끈기 목적 성취 풍요 성공 숙달 능력 에너지 소명 침묵 직관 재생 소생' 등
///
/// 선택 : 게임 시작과 동시에 나타났던 빛의 구체와 문자를 기억해보세요.
/// 1) 선택지1 : 정답  (위치는 1/2확률로 랜덤)
/// 2) 선택지2 : 오답  (위치는 1/2확률로 랜덤)  
/// 
/// </summary>



/// ※ 문제 패널 생성 로직
/// | Easy / Normal : 왼쪽엔 Light, 오른쪽엔 글귀 고정으로 출현
/// | Hard : 각각 Light와 글귀가 랜덤 위치로 출현

/// ※ 정답 패널 생성 로직
/// | 빛 - 정답과 오답이 같아도 상관 없다
/// | 글귀 - 문제 패널의 글귀와 하나는 일치해야 함

using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel; // 질문 패널



    public static QuizManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        //if (GameManager.instance.isStart)
    }

    /// <summary>
    /// ※ 문제 패널 생성 로직
    /// | Easy / Normal : 왼쪽엔 Light, 오른쪽엔 글귀 고정으로 출현
    /// | Hard : 각각 Light와 글귀가 랜덤 위치로 출현
    /// </summary>
    /// <param name=""></param>
    void QuizSuggestion()
    {
        //quizThemeChoice = Random.Range(0, 2);

        //switch (quizThemeChoice)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        break;
        //}
    }

    /** 인수를 주지 않았을때의 생성자 **/
    //public void QuizUnit()
    //{
    //    Random random = Random.range;           /// 랜덤함수 객체생성
        
    //    this.color = random.Range(7);           /// 0~6 까지의 임의의 정수 생성
    //    this.letter = random.Range(4);          /// 0~3 까지의 임이의 정수 생성
    //}

    /** 인수를 주었을때의 생성자 **/
    //public QuizUnit(int _color, int _letter)
    //{
    //    Random randdom = new Random();

    //    do
    //    {
    //        this.color = random.Range(7);
    //        this.letter = random.Range(4);
    //    } while (this.color == _color && this.letter == _letter)        /// 둘다 동일하다면 새로운 랜덤 생성. 둘 중 하나라도 동일하지 않을때까지 계속돌림	
    //}

    //QuizUnit answer;
    //QuizUnit opt1;
    //QuizUnit opt2;

    //public void Play()
    //{
    //    answer = new Quiz();                                        ///정답 생성

    //    Random random = new Random();

    //    random = random.Range(2);                                   /// 1/2확률로 랜덤 범위 지정 - 하나가 정답이면 다른 하나는 오답
    //    switch (random)
    //    {
    //        case 0:
    //            {
    //                opt1 = answer;
    //                opt2 = new QuizUnit(answer.color, answer.letter);   ///옵션2는 새로운 임의의 색상/문자 생성
    //                break;
    //            }
    //        case 1:
    //            {
    //                opt2 = answer;
    //                opt1 = new QuizUnit(answer.color, answer.letter);
    //                break;
    //            }
    //    }

    //    //1. 문제지 - 제일 처음 정답을 먼저 보여줘야 함
    //    유니티화면표시될함수(answer.color);
    //    유니티화면표시될함수(answer.letter);

    //    //2. 선택지
    //    random = random.Range(2);

    //    ConsoleKeyInfo KeyInfo;                         ///입력객체 -> 유니티에 맞게 변경
    //    KeyInfo = Console.ReadKey();                    ///입력객체 -> 유니티에 맞게 변경
    //                                                    /// if(KeyInfo.Key == ConsoleKey.A)

    //    int choice;

    //    switch (random)   /// 처음 문제는 ans의 color와 letter 둘중 하나만 랜덤으로 제시해야 한다.	
    //    {
    //        case 0:
    //            {
    //                /// 문제 제시
    //                유니티화면표시(answer.color);

    //                /// 선택지 제시
    //                유니티화면표시(opt1.letter);
    //                유니티화면표시(opt2.letter);

    //                /// 유저 선택 및 결과 표시
    //                System.Console.WriteLine("선택하세요");
    //                choice = Convert.ToInt32(Console.ReadKey());       ///입력개체 -> 유니티에 맞게 변경
    //                if (choice == answer.letter)
    //                    System.Console.WriteLine("정답입니다");
    //                else
    //                    System.Console.WriteLine("오답입니다.");
    //            }
    //        case 1:
    //            {
    //                /// 문제 제시
    //                유니티화면표시(answer.letter);

    //                /// 선택지 제시
    //                유니티화면표시(opt1.color);
    //                유니티화면표시(opt2.color);

    //                /// 유저 선택 및 결과 표시
    //                System.Console.WriteLine("선택하세요");
    //                choice = Convert.ToInt32(Console.ReadKey());       ///입력개체 -> 유니티에 맞게 변경
    //                if (choice == answer.color)
    //                    System.Console.WriteLine("정답입니다");
    //                else
    //                    System.Console.WriteLine("오답입니다.");
    //            }
    //    }
    //}
}