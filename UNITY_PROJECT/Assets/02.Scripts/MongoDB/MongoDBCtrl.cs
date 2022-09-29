/// <summary>
/// 클라이언트 내부 → 데이터베이스
/// 데이터베이스 내부 → 콜렉션
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoDBCtrl : MonoBehaviour
{
    // Get Connect(User) Info
    const string MONGO_URI =
        "mongodb+srv://song:song@cluster0.qs1ppkp.mongodb.net/?retryWrites=true&w=majority";
    MongoClient client;

    // Get Database Info
    const string DATABASE_NAME = 
        "TestDB";
    IMongoDatabase db;

    // - 예습에선 콜렉션이 하나이므로 전역변수로 관리
    // - DB 안에선 여러 콜렉션이 있을 땐 내부 함수에서 가져오는걸 권장
    // - GameData Class의 형태로 생성
    IMongoCollection<GameData> db_col;

    private void Start()
    {
        First();
        Debug.Log("=====:Action:=====");
        //DB_Find("song");
        //DB_All_View();
        //DB_Insert("ugi", 1);
        //DB_Remove("song");
    }

    void First()
    {
        DB_Login();
        Get_DataBase();
        Get_Collection();

        Debug.Log("=====:접속 정보:=====");
        Debug.Log("Client : " + client);
        Debug.Log("DataBase Name : " + db);
        Debug.Log("Data Collection : " + db_col);
    }
    void DB_Login()       { client = new MongoClient(MONGO_URI); }
    void Get_DataBase()   { db = client.GetDatabase(DATABASE_NAME); }
    void Get_Collection() { db_col = db.GetCollection<GameData>("TestDB.col"); }

    void DB_Insert(string name, int score)
    {
        // MongoDB 내에서 name필드에 중복이 있는지 검사
        if (db_exist(name))
        {
            Debug.Log("Name is Exist : " + name);
            return;
        }

        GameData _GameData = new GameData(); // 빈 데이터
        //_GameData.name = name;

        db_col.InsertOne(_GameData);
    }

    bool db_exist(string name)
    {
        BsonDocument _bson = new BsonDocument { { "name", name } };
        List<GameData> user_list = db_col.Find(_bson).ToList();
        GameData[] user_data = user_list.ToArray();
        if (user_data.Length == 0) return false; // 데이터가 없다면 false
        return true;
    }

    void DB_All_View()
    {
        List<GameData> user_list = db_col.Find(user => true).ToList(); // 콜렉션을 리스트화 한다.
        GameData[] user_data = user_list.ToArray(); // 최적화를 위해 List 자료형 -> 배열로 변환
        for (int i = 0; i < user_data.Length; i++)
        {
            //Debug.Log(user_data[i].name + " : " + user_data[i].score);
        }
    }

    void DB_Find(string name)
    {
        // 데이터를 주고 받는 일은 json으로 보내어 보안을 강화한다.
        BsonDocument _bson = new BsonDocument { { "name", name } };
        List<GameData> user_list = db_col.Find(_bson).ToList();
        GameData[] user_data = user_list.ToArray();
        for (int i = 0; i < user_data.Length; i++)
        {
            //Debug.Log(user_data[i].name + " : " + user_data[i].score);
        }
    }

    void DB_Remove(string name) // 한 필드만 삭제
    {
        BsonDocument _bson = new BsonDocument { { "name", name } };

        db_col.DeleteOne(_bson);
    }

    void DB_Removes(string name) // 동일 name의 필드들을 삭제
    {
        BsonDocument _bson = new BsonDocument { { "name", name } };

        db_col.DeleteMany(name);
    }

    void DB_Remove_All() // 전체 필드 삭제
    {
        BsonDocument _bson = new BsonDocument { }; // <- 모든 선택

        db_col.DeleteMany(_bson);
    }

    void DB_Update(string name, int score)
    {
        BsonDocument _bson_search = new BsonDocument { { "name", name } }; // 조회
        BsonDocument _bson_update = new BsonDocument { { "name", name }, { "score", score } }; // Update

        db_col.FindOneAndUpdate(_bson_search, _bson_update);
    }
}