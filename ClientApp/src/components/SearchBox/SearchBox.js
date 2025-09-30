//ReactオブジェクトのuseEffectメソッドを同名のuseEffect変数に分割代入して
//useEffectメソッドをReact.useEffectとせずに使用できるようにした
import React, { useState } from 'react';
import SearchIcon from '../../images/SearchIcon.svg';
import './SearchBox.css';

const SearchBox = ({ onSearch, onFavoriteRestaurant }) => { //onSearchメソッド（=handleSearchResults）は親からこの入れ物を使ってといわんばかりに渡される

    const [keyword, setKeyword] = useState('');

    // APIにキーワードを送信(軽量なやりとりはfetchつかう)
    const sendSearchRequest = async () => {
        if (keyword.trim() === '') return; // 空の入力は無視
        try {
            // 現在地を取得
            navigator.geolocation.getCurrentPosition(
                async (position) => {
                    const { latitude, longitude } = position.coords;

                    // APIリクエスト送信
                    const response = await fetch('searchdata', {
                        method: 'POST',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({
                            keyword: keyword,
                            latitude: latitude,
                            longitude: longitude
                        })
                    });

                    if (!response.ok) {
                        throw new Error('APIリクエストに失敗しました');
                    }

                    const result = await response.json();
                    console.log('結果:', result.places);
                    onSearch(result.places);
                },
                (error) => {
                    console.error('位置情報の取得に失敗しました:', error);
                    alert('位置情報が取得できませんでした。ブラウザの設定を確認してください。');
                }
            );

            const response = await fetch('searchdata', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json', //追加
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ keyword: keyword })
            });

            if (!response.ok) {
                throw new Error('APIリクエストに失敗しました');
            }
            const result = await response.json();
            console.log('結果:', result.places);
            onSearch(result.places); // onSearchメソッドに値を詰めて親コンポーネントに結果を渡す(=handleSearchResults(result.Feature)→setSearchResults(result.Feature))
        } catch (error) {
            console.error('エラーが発生しました:', error);
        }

        //try {
        //    // ここでマイレストラン情報を取得
        //    const getFavoriteRestaurantData = await fetch('getFavoriteRestaurant', {
        //        method: 'GET',
        //        headers: {
        //            'Accept': 'application/json'
        //        }
        //    });

        //    if (getFavoriteRestaurantData.ok) {
        //        const FavoriteRestaurantData = await getFavoriteRestaurantData.json();
        //        console.log('マイレストラン情報:', FavoriteRestaurantData);
        //        // 取得したマイレストラン情報を用いて、例えば「+」を「-」に変更する処理を実施する
        //        onFavoriteRestaurant(FavoriteRestaurantData);
        //    } else {
        //        console.error('マイレストラン情報を取得できませんでした。');
        //    }
        //}
        //catch (error) {
        //    console.error('通信エラー:', error);
        //}
        
    };

    // Enterキー押下時に検索実行
    const handleKeyDown = (e) => {
        if (e.key === 'Enter') {
            sendSearchRequest();
        }
    };

    // サーチアイコンクリック時に検索実行
    const handleIconClick = () => {
        sendSearchRequest();
    };

    return (
        <div id="search-container">
            <div id="search-wrapper">
                <input
                    type="text"
                    className="original-input"
                    id="search-input"
                    placeholder="キーワードを入力してください"
                    value={keyword}
                    onChange={(e) => setKeyword(e.target.value)}
                    onKeyDown={handleKeyDown} // Enterキーのイベント
                />
                <img
                    src={SearchIcon}
                    alt="Search Icon"
                    id="search-icon"
                    onClick={handleIconClick} // クリックイベント
                    style={{ cursor: 'pointer' }} // アイコンをクリック可能にするためのスタイル
                />
            </div>
        </div>
    );
}
//defaultだとexportされるのが一個のみで確定なので
//import時に名前を自由に変えられる
export default SearchBox; //原則1ファイル1関数コンポーネント
