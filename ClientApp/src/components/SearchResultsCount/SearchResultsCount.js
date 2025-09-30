//ReactオブジェクトのuseEffectメソッドを同名のuseEffect変数に分割代入して
//useEffectメソッドをReact.useEffectとせずに使用できるようにした
import './SearchResultsCount.css';

const SearchResultsCount = ({ searchResultsCount }) => {

    if (searchResultsCount == 0) {
        return (
            <></>
        );
    } else {
        if (searchResultsCount === undefined) {
            return (
                <div id="searchResultsCount-container">
                    <div id="searchResultsCount-wrapper">
                        <div className="searchResultsCount">
                            検索結果：0件
                        </div>
                    </div>
                </div>
            );
        } else {
            return (
                <div id="searchResultsCount-container">
                    <div id="searchResultsCount-wrapper">
                        <div className="searchResultsCount">
                            検索結果：{searchResultsCount}件
                        </div>
                    </div>
                </div>
            );
        }
        
    }


}
//defaultだとexportされるのが一個のみで確定なので
//import時に名前を自由に変えられる
export default SearchResultsCount; //原則1ファイル1関数コンポーネント