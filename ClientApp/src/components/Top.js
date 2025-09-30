//ReactオブジェクトのuseEffectメソッドを同名のuseEffect変数に分割代入して
//useEffectメソッドをReact.useEffectとせずに使用できるようにした
import React, { useState, useEffect } from 'react';
import Map from './Map/Map';
import SearchBox from './SearchBox/SearchBox';
import SearchResults from './SearchResults/SearchResults';
import SearchResultsCount from './SearchResultsCount/SearchResultsCount';
import ReviewModal from "./modals/ReviewModal/ReviewModal";
import ModalPortal from "./common/ModalPortal";
import './Top.css';

const Top = () => {
    const [searchResults, setSearchResults] = useState([]); // setSearchResults関数にあたらしい値をいれる [検索結果,検索結果をセットする関数]
    const [selectedRestaurant, setSelectedRestaurant] = useState(null); // [クリックされたカードの飲食店,クリックされたカードの飲食店をセットする関数]
    const [FavoriteRestaurants, setFavoriteRestaurants] = useState([]); // マイレストラン情報
    const [reviewedRestaurants, setReviewedRestaurants] = useState([]); // 訪問済みレストランの状態

    const getFavoriteRestaurants = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await fetch('getFavoriteRestaurants', {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Authorization': `Bearer ${token}`,
                    //'Content-Type': 'application/json', // GetはvalueをAPI側に送信しないため不要
                }
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
                // ここでは、resultがレストラン名の配列やオブジェクトの配列であると想定

                let favorites;
                if (Array.isArray(result.userFavoriteRestaurants)) {
                    // すでに配列ならそのまま
                    favorites = result.userFavoriteRestaurants;
                } else if (result.userFavoriteRestaurants) {
                    //配列ではなくて、かつ 単一オブジェクトが来たら長さ1の配列に
                    favorites = [result.userFavoriteRestaurants];
                } else {
                    // null / undefined の場合は空配列に
                    favorites = [];
                }

                setFavoriteRestaurants(favorites);
            }            
        } catch (error) {
            console.error('エラーが発生しました:', error);
            //alert('マイリストの取得に失敗しました');
        }
    };

    const getReviewedRestaurants = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await fetch('userreviewrestaurant/getreviewedlist', {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Authorization': `Bearer ${token}`,
                    //'Content-Type': 'application/json', // GetはvalueをAPI側に送信しないため不要
                }
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
                // ここでは、resultがレストラン名の配列やオブジェクトの配列であると想定
                let reviews;
                if (Array.isArray(result.userReviewRestaurant)) {
                    // すでに配列ならそのまま
                    reviews = result.userReviewRestaurant;
                } else if (result.userReviewRestaurant) {
                    //配列ではなくて、かつ 単一オブジェクトが来たら長さ1の配列に
                    reviews = [result.userReviewRestaurant];
                } else {
                    // null / undefined の場合は空配列に
                    reviews = [];
                }

                setReviewedRestaurants(reviews);
            }
        } catch (error) {
            console.error('エラーが発生しました:', error);
            //alert('マイリストの取得に失敗しました');
        }
    };

    // --- モーダル用 state ---
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [modalPlace, setModalPlace] = useState("");
    // モーダルを開く
    const openReviewModal = (placeName) => {
        setModalPlace(placeName);
        setIsModalOpen(true);
    };
    // モーダルを閉じる
    const closeReviewModal = () => setIsModalOpen(false);

    // トップページアクセス時に非同期でいいね済みのレストランを取得する
    useEffect(() => {
    
        getFavoriteRestaurants();

        getReviewedRestaurants();

    }, []); // 空の依存配列なので、初回マウント時のみ実行

    const handleSearchResults = (results) => {
        setSearchResults(results);
    };

    // SearchResultsのカードでクリックされた項目を取得
    const handleCardClick = (restaurantName) => {
        // 既に選択されている場合、一旦リセットしてから設定 → 店A→現在地取得ボタン→店Aの場合に対応
        if (selectedRestaurant === restaurantName) {
            setSelectedRestaurant(null); // リセット
            setTimeout(() => setSelectedRestaurant(restaurantName), 0); // 同じ飲食店を再度選択
        } else {
            setSelectedRestaurant(restaurantName);
        }
    };

    // Markerでクリックされた項目を取得
    const handleMarkerClick = (restaurantName) => {
        setSelectedRestaurant(restaurantName);
    };

    return (
        <div id="top-container">
            <h1>Map!</h1>           
            <SearchBox
                onSearch={handleSearchResults}
                onFavoriteRestaurant={setFavoriteRestaurants}>
            </SearchBox>{/*onSearch という propsが渡され、その props に handleSearchResults 関数が紐づけられている。SearchBoxで使う関数名にonSearchを指名している*/}
            <SearchResultsCount searchResultsCount={searchResults === undefined ? undefined : searchResults.length}></SearchResultsCount>
            <div id="top-content">
                <Map
                    results={searchResults}
                    selectedRestaurant={selectedRestaurant}
                    onMarkerClick={handleMarkerClick}>
                </Map>{/*次はsearchResultsをMapに渡す（緯度経度の値を参照したいから）*/}
                <SearchResults
                    results={searchResults}
                    selectedRestaurant={selectedRestaurant}
                    onCardClick={handleCardClick}
                    FavoriteRestaurants={FavoriteRestaurants}
                    getFavoriteRestaurants={getFavoriteRestaurants}/*関数を渡す*/
                    reviewedRestaurants={reviewedRestaurants}
                    getReviewedRestaurants={getReviewedRestaurants}/*関数を渡す*/
                    // ボタンの onClick で openReviewModal を呼び出せるように prop 追加
                    onReviewClick={openReviewModal}
                >
                </SearchResults>
                {/* ここにモーダルをレンダリング */}
                {isModalOpen
                    ? ( //三項演算子
                    <ModalPortal>
                        <ReviewModal
                            placeName={modalPlace}
                            onClose={closeReviewModal}
                            getReviewedRestaurants={getReviewedRestaurants}
                        />
                    </ModalPortal>
                    )
                    : null
                }
            </div>
        </div>
    );
}
//defaultだとexportされるのが一個のみで確定なので
//import時に名前を自由に変えられる
export default Top ; //原則1ファイル1関数コンポーネント
