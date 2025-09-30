import React, { useState, useEffect } from "react";
import "./ReviewModal.css";

export default function ReviewModal({ placeName, onClose, getReviewedRestaurants }) {
    // 点数（整数部・小数部）用 state
    const [whole, setWhole] = useState("0");      // 一の位
    const [decimal, setDecimal] = useState("0");  // 小数点第一位

    // 各評価用 state
    const [taste, setTaste] = useState("");
    const [costPerf, setCostPerf] = useState("");
    const [service, setService] = useState("");
    // 口コミコメント
    const [comment, setComment] = useState("");

    // 選択肢データ
    const onesPlaceOptions = Array.from({ length: 6 }, (_, i) => i.toString());  // 0〜5
    const firstDecimalOptions = Array.from({ length: 10 }, (_, i) => i.toString()); // 0〜9
    const levelOptions = ["わるい", "ふつう", "ぐっと", "すーぱー"];

    // トップページアクセス時に非同期で
    useEffect(() => {

        //getFavoriteRestaurants();
        getMyReview();


    }, []); // 空の依存配列なので、初回マウント時のみ実行

    const getMyReview = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await fetch('/userreviewrestaurant/get', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json', // API側に値を送信する際に必要
                },
                body: JSON.stringify({ keyword: placeName }), // APIに送るデータ
            }); // APIエンドポイント例

            // 認証エラーの場合は特に何もしない
            if (response.status === 401) {
                //window.location.href = '/login';
                //return;
            }
            else {
                if (!response.ok) {
                    throw new Error('APIリクエストに失敗しました');
                }
                const result = await response.json();
                // resultを各項目に代入したい
                const [intPart, decPart] = result.totalScore.toString().split('.');
                setWhole(intPart);                           // 例: "3"
                setDecimal(decPart || "0");                  // 例: "4"
                // 2. taste, costPerformance, service は数値インデックスなので
                //    levelOptions 配列から文字列に戻す
                setTaste(levelOptions[result.taste] || "");
                setCostPerf(levelOptions[result.costPerformance] || "");
                setService(levelOptions[result.hospitality] || "");

                // 3. comment
                setComment(result.review || "");
            }
        } catch (error) {
            console.error('エラーが発生しました:', error);
            //alert('マイリストの取得に失敗しました');
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const score = parseFloat(`${whole}.${decimal}`);

        var tasteIndex = levelOptions.indexOf(taste);         // taste が "" なら -1

        if (tasteIndex === -1) {
            tasteIndex = 0;
        }

        var costPerfIndex = levelOptions.indexOf(costPerf);         // costPerf が "" なら -1
        if (costPerfIndex === -1) {
            costPerfIndex = 0;
        }

        var serviceIndex = levelOptions.indexOf(service);         // service が "" なら -1
        if (serviceIndex === -1) {
            serviceIndex = 0;
        }

        const payload = {
            place: placeName,
            score,
            taste: tasteIndex,
            costPerformance: costPerfIndex,
            service: serviceIndex,
            comment,
        };

        try {
            const token = localStorage.getItem("token");
            const response = await fetch("/userreviewrestaurant/add", {
                method: "POST",
                headers: {
                    //'Accept': 'application/json', //追加
                    'Content-Type': 'application/json', // JSON形式のデータを送信
                    'Authorization': `Bearer ${token}`,
                },
                body: JSON.stringify(payload),
            });

            //const data = await response.json();     // これでエラー詳細(JSON)を取得

            // 認証エラーの場合はログインページへリダイレクト
            if (response.status === 401) {
                window.location.href = '/login';
                return;
            }

            if (response.ok) {
                //onClose();
                getReviewedRestaurants()
                alert("口コミ登録しました");
            } else {
                alert("送信に失敗しました：" + response.statusText);
            }
        } catch (err) {
            console.error(err);
            alert("ネットワークエラーが発生しました");
        }
    };

    return (
        <>
            <h2>{placeName} の口コミ投稿</h2>
            <form onSubmit={handleSubmit}>
                {/* 点数 */}
                <div className="form-group">
                    <label>点数</label>
                    <div className="score-select">
                        <select value={whole} onChange={(e) => setWhole(e.target.value)}>
                            {onesPlaceOptions.map((o) => (
                                <option key={o} value={o}>
                                    {o}
                                </option>
                            ))}
                        </select>
                        <span className="dot">.</span>
                        <select
                            value={decimal}
                            onChange={(e) => setDecimal(e.target.value)}
                        >
                            {firstDecimalOptions.map((o) => (
                                <option key={o} value={o}>
                                    {o}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                {/* 味 */}
                <div className="form-group">
                    <label className="checkbox-title" >味</label>
                    <div className="checkbox-group">
                        {levelOptions.map((o) => (
                            <label key={o} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    value={o}
                                    checked={taste === o}
                                    // もし同じ o をクリックしたら “”（未選択）に、違う o ならその o にセット
                                    onChange={() => setTaste(taste === o ? "" : o)}
                                />
                                <span className="checkbox-text">{o}</span>
                            </label>
                        ))}
                    </div>
                </div>

                {/* コスパ */}
                <div className="form-group">
                    <label className="checkbox-title">コスパ</label>
                    <div className="checkbox-group">
                        {levelOptions.map((o) => (
                            <label key={o} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    name="costPerf"
                                    value={o}
                                    checked={costPerf === o}
                                    onChange={() => setCostPerf(costPerf === o ? "" : o)}
                                />
                                <span className="checkbox-text">{o}</span>
                            </label>
                        ))}
                    </div>
                </div>

                {/* 接客 */}
                <div className="form-group">
                    <label className="checkbox-title">接客</label>
                    <div className="checkbox-group">
                        {levelOptions.map((o) => (
                            <label key={o} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    name="service"
                                    value={o}
                                    checked={service === o}
                                    onChange={() => setService(service === o ? "" : o)}
                                />
                                <span className="checkbox-text">{o}</span>
                            </label>
                        ))}
                    </div>
                </div>

                {/* 自由コメント */}
                <div className="form-group">
                    <label className="checkbox-title">口コミ</label>
                    <textarea
                        rows="5"
                        value={comment}
                        onChange={(e) => setComment(e.target.value)}
                        placeholder="自由にご記入ください"
                    />
                </div>

                {/* ボタン */}
                <div className="button-group">
                    <button type="submit">送信</button>
                    <button type="button" onClick={onClose}>
                        キャンセル
                    </button>
                </div>
            </form>

        </>

    );
}
