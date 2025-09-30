import React, { useEffect, useRef } from 'react';//useRefは再レンダリングされてもリセットされない値を保持するためのフック
import Compass from '../../images/Compass.svg';
import './Map.css';

const Map = ({ results, selectedRestaurant, onMarkerClick }) => { //Mapコンポーネント単位でレンダリングをおこなう(results,selectedRestaurantのpropsの更新タイミングで)
    const mapRef = useRef(null); // マップインスタンスを保持するためのref
    const markersRef = useRef([]); // マーカーを保持するためのref
    let currentLocationMarker = useRef(null); // 現在地マーカーの管理

    // Leafletのマップを初期化する関数
    const initializeMap = () => {
        const L = window.L;
        const map = L.map('map').setView([35.6895, 139.6917], 16); // 東京を中心に設定
        mapRef.current = map; // マップインスタンスをrefに保存

        //L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        //    attribution: '© OpenStreetMap contributors',
        //}).addTo(map);

        L.tileLayer('https://mt1.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
            attribution: '© Google',
        }).addTo(map);

        return map;
    };

    // 現在地取得処理
    /*
     map.locate() が現在地を取得し、それを onLocationFound で受け取った後に 
     map.setView() で地図の表示を更新する、という流れ
     */
    const setupLocationTracking = (map) => {
        const L = window.L;

        const onLocationFound = (e) => {
            const currentZoom = map.getZoom();
            map.setView(e.latlng, currentZoom);

            if (currentLocationMarker.current) {
                map.removeLayer(currentLocationMarker.current);
            }
            currentLocationMarker.current = L.marker(e.latlng)
                .addTo(map)
                .bindPopup('現在地')
                .openPopup();
        };

        const onLocationError = (e) => {
            console.log('現在地取得に失敗: ' + e.message);
            map.setView([35.682839, 139.759455], 13); // 東京の緯度経度
        };

        map.on('locationfound', onLocationFound);
        map.on('locationerror', onLocationError);

        map.locate({ setView: false, maxZoom: map.getZoom() });
    };

    // 現在地取得用のボタンを設定
    const addLocateControl = (map) => {
        const L = window.L;

        const locateControl = L.Control.extend({
            options: { position: 'topright' },
            onAdd: function () {
                const container = L.DomUtil.create('div', 'locate-button');
                const img = L.DomUtil.create('img', '', container);
                img.src = Compass;
                img.alt = 'Locate';

                container.onclick = () => {
                    //map.locate({ setView: true, maxZoom: 16 });
                    setupLocationTracking(map); // カスタム処理を含めて現在地取得を実行
                };

                return container;
            },
        });

        map.addControl(new locateControl());
    };

    // マップ初期化と現在地取得のセットアップ
    useEffect(() => {//引き数なしパターン
        const L = window.L;
        if (!L) {
            console.error('Leaflet library is not loaded.');
            return;
        }

        // マップがすでに初期化されていなければ初期化する
        if (!mapRef.current) {
            const map = initializeMap();
            setupLocationTracking(map); // 現在地取得のセットアップ
            addLocateControl(map); // カスタムコントロールを追加
        }
    }, []); // マップ初期化は一度だけ行う（コンポーネントが最初に表示されたときだけ実行）

    // resultsに更新があった時の処理
    useEffect(() => {
        const L = window.L;
        const map = mapRef.current;

        if (results) {
            // 古いマーカーを削除
            markersRef.current.forEach((marker) => map.removeLayer(marker));
            markersRef.current = [];

            // 新しいマーカーを追加
            results.forEach((result) => {
                const [lat, lng] = [result.location.latitude, result.location.longitude];;
                const marker = L.marker([lat, lng])
                    .addTo(map)
                    .bindPopup(`<h3>${result.displayName.text}</h3><p>${result.formattedAddress}</p>`);

                // マーカーにクリック（タップ）イベントを設定
                marker.on('click', () => {
                    //console.log(`マーカー ${result.Name} がクリックされました`);
                    // 必要な処理をここに記述
                    onMarkerClick(result.displayName.text); // マーカークリックでonMarkerClickを呼ぶ
                });

                // マーカーを配列に保存
                markersRef.current.push({ marker, customLabel: result.displayName.text });
            });
        }
    }, [results]); // 検索結果の変更を監視

    // クリックされたレストラン名でポップアップを開く
    useEffect(() => {
        if (selectedRestaurant) {
            const selectedMarker = markersRef.current.find((m) => m.customLabel === selectedRestaurant);
            if (selectedMarker) {
                selectedMarker.marker.openPopup(); // ポップアップを表示とともに地図も移動してくれる
                mapRef.current.setView(selectedMarker.marker.getLatLng(), mapRef.current.getZoom()); // 選択されたマーカーに地図を移動
            }
        }
    }, [selectedRestaurant]); // 選択された飲食店を監視

    return <div id="map"></div>;
};

export default Map;


