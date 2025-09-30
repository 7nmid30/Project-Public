//ReactオブジェクトのuseEffectメソッドを同名のuseEffect変数に分割代入して
//useEffectメソッドをReact.useEffectとせずに使用できるようにした
import React, { useState } from 'react';
import './SearchResults.css';
//import Heart from '../../images/Heart.svg';
// こうすると <HeartIcon /> が純粋な <svg> 要素として埋め込まれる
//import { ReactComponent as Heart } from '../../images/Heart.svg';
import Heart from '../../icons/Heart';
//import Footprints from '../../images/Footprints.svg';
import Footprint from '../../icons/Footprint';

const SearchResults = ({ results, onCardClick, selectedRestaurant, FavoriteRestaurants, getFavoriteRestaurants, reviewedRestaurants, getReviewedRestaurants, onReviewClick }) => {

    //const addToMyVisitedList = async (name) => {
    //    if (name.trim() === '') return; // 空の入力は無視
    //    try {
    //        const token = localStorage.getItem("token");
    //        const response = await fetch('addtovisitedrestaurantlist', {
    //            method: 'POST', // APIのHTTPメソッド
    //            headers: {
    //                //'Accept': 'application/json', //追加
    //                'Content-Type': 'application/json', // JSON形式のデータを送信
    //                'Authorization': `Bearer ${token}`,
    //            },
    //            body: JSON.stringify({ name: name }), // APIに送るデータ
    //        });

    //        // 認証エラーの場合はログインページへリダイレクト
    //        if (response.status === 401) {
    //            window.location.href = '/login';
    //            return;
    //        }

    //        if (!response.ok) {
    //            throw new Error('APIリクエストに失敗しました');
    //        }

    //        const result = await response.json();
    //        console.log('APIレスポンス:', result);
    //        alert('いったことあるリストに登録成功！');
    //    } catch (error) {
    //        console.error('エラーが発生しました:', error);
    //        //alert('登録に失敗しました');
    //    }
    //};
    //const onClickFootPrintIcon = async (name) => {
    //    if (name.trim() === '') return; // 空の入力は無視

    //    // 現在のvisitedRestaurantsを参照して追加か削除かを判定するフラグを作成
    //    const isAdding = Array.isArray(visitedRestaurants) && !visitedRestaurants.some(res => res.restaurantName === name);

    //    if (isAdding) {
    //        // 追加（登録）のAPI呼び出しを実施する
    //        try {
    //            const token = localStorage.getItem("token");
    //            const response = await fetch('visitedrestaurantlist/add', {
    //                method: 'POST', // APIのHTTPメソッド
    //                headers: {
    //                    //'Accept': 'application/json', //追加
    //                    'Content-Type': 'application/json', // JSON形式のデータを送信
    //                    'Authorization': `Bearer ${token}`,
    //                },
    //                body: JSON.stringify({ keyword: name }), // APIに送るデータ
    //            });

    //            // 認証エラーの場合はログインページへリダイレクト
    //            if (response.status === 401) {
    //                window.location.href = '/login';
    //                return;
    //            }

    //            if (!response.ok) {
    //                throw new Error('APIリクエストに失敗しました');
    //            }

    //            const result = await response.json();
    //            console.log('APIレスポンス:', result);
    //            alert('いったことあるレストランリストに登録成功！');

    //        } catch (error) {
    //            console.error('エラーが発生しました:', error);
    //            alert('登録に失敗しました');
    //        }
    //    } else {
    //        //削除のAPI呼び出しを実施する
    //        try {
    //            const token = localStorage.getItem("token");
    //            const response = await fetch('visitedrestaurantlist/delete', {
    //                method: 'DELETE', // APIのHTTPメソッド
    //                headers: {
    //                    //'Accept': 'application/json', //追加
    //                    'Content-Type': 'application/json', // JSON形式のデータを送信
    //                    'Authorization': `Bearer ${token}`,
    //                },
    //                body: JSON.stringify({ keyword: name }), // APIに送るデータ
    //            });

    //            // 認証エラーの場合はログインページへリダイレクト
    //            if (response.status === 401) {
    //                window.location.href = '/login';
    //                return;
    //            }

    //            if (!response.ok) {
    //                throw new Error('APIリクエストに失敗しました');
    //            }

    //            const result = await response.json();
    //            console.log('APIレスポンス:', result);
    //            alert('いったことあるレストランリストからの削除成功！');

    //        } catch (error) {
    //            console.error('エラーが発生しました:', error);
    //            alert('削除に失敗しました');
    //        }
    //    }

    //    getVisitedRestaurants()

    //};

    const onClickHeartIcon = async (name) => {
        if (name.trim() === '') return; // 空の入力は無視

        // 現在のFavoriteRestaurantsを参照して追加か削除かを判定するフラグを作成
        const isAdding = Array.isArray(FavoriteRestaurants) && !FavoriteRestaurants.some(res => res.restaurantName === name);
        
        if (isAdding) {
            // 追加（登録）のAPI呼び出しを実施する
            try {
                const token = localStorage.getItem("token");
                const response = await fetch('FavoriteRestaurantlist/add', {
                    method: 'POST', // APIのHTTPメソッド
                    headers: {
                        //'Accept': 'application/json', //追加
                        'Content-Type': 'application/json', // JSON形式のデータを送信
                        'Authorization': `Bearer ${token}`,
                    },
                    body: JSON.stringify({ keyword: name }), // APIに送るデータ
                });

                // 認証エラーの場合はログインページへリダイレクト
                if (response.status === 401) {
                    window.location.href = '/login';
                    return;
                }

                if (!response.ok) {
                    throw new Error('APIリクエストに失敗しました');
                }

                const result = await response.json();
                console.log('APIレスポンス:', result);
                alert('マイレストランリストに登録成功！');

            } catch (error) {
                console.error('エラーが発生しました:', error);
                alert('登録に失敗しました');
            }
        } else {
            //削除のAPI呼び出しを実施する
            try {
                const token = localStorage.getItem("token");
                const response = await fetch('FavoriteRestaurantlist/delete', {
                    method: 'DELETE', // APIのHTTPメソッド
                    headers: {
                        //'Accept': 'application/json', //追加
                        'Content-Type': 'application/json', // JSON形式のデータを送信
                        'Authorization': `Bearer ${token}`,
                    },
                    body: JSON.stringify({ keyword: name }), // APIに送るデータ
                });

                // 認証エラーの場合はログインページへリダイレクト
                if (response.status === 401) {
                    window.location.href = '/login';
                    return;
                }

                if (!response.ok) {
                    throw new Error('APIリクエストに失敗しました');
                }

                const result = await response.json();
                console.log('APIレスポンス:', result);
                alert('マイレストランリストからの削除成功！');

            } catch (error) {
                console.error('エラーが発生しました:', error);
                alert('削除に失敗しました');
            }
        }

        getFavoriteRestaurants()
        
    };

    if (!results || results.length === 0) {
        return (
            <></>           
        );
    }

    return (
        <div id="searchResults-container">
            <div id="searchResults-wrapper">
                {results.map((result, index) => {
                    const isFavorited = Array.isArray(FavoriteRestaurants)
                        && FavoriteRestaurants.some(res => res.restaurantName === result.displayName.text);
                    const isReviewed = Array.isArray(reviewedRestaurants)
                        && reviewedRestaurants.some(res => res.restaurantName === result.displayName.text);
                    return (
                        <div
                            key={index}
                            className={`card ${selectedRestaurant === result.displayName.text ? 'selected' : ''}`}
                            onClick={() => onCardClick(result.displayName.text)}
                            style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
                        >
                            <div>
                                <h3>{result.displayName.text}</h3>
                                <p>{result.formattedAddress}</p>
                                <p>{result.rating}</p>
                            </div>
                            <div id="restaurantListIcon-wrapper">
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        //onClickFootPrintIcon(result.displayName.text);
                                        onReviewClick(result.displayName.text);
                                    }}
                                >
                                    <Footprint alt="Footprint Icon" className={`restaurant-list-icon footprint-icon ${isReviewed ? 'reviewed' : ''}`} />
                                </button>
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        onClickHeartIcon(result.displayName.text);
                                    }}
                                >
                                    <Heart alt="Heart Icon" className={`restaurant-list-icon heart-icon ${isFavorited ? 'favorited' : ''}`} />
                                </button>
                            </div>
                        </div>
                    );
                })}
            </div>
        </div>
    );
}
//defaultだとexportされるのが一個のみで確定なので
//import時に名前を自由に変えられる
export default SearchResults; //原則1ファイル1関数コンポーネント
