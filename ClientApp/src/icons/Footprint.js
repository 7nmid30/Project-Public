import React from 'react';

/**
 * Footprint SVG React component
 * Props:
 *  - className: optional CSS class
 *  - width: optional width (default: 24)
 *  - height: optional height (default: 24)
 */
export default function Footprint({ className, width = 24, height = 24 }) {
    return (
        <svg xmlns="http://www.w3.org/2000/svg"
            className={className}
            width={width}
            height={height}
            viewBox="0 0 200 200"
            xmlns="http://www.w3.org/2000/svg"
            fill="currentColor">
            <path d="M66.216,0a21.931,21.931,0,1,0,.268,0Zm67.3,0a21.793,21.793,0,1,0,21.8,21.791A21.8,21.8,0,0,0,133.518,0ZM20.2,47.35a20.2,20.2,0,1,0,20.2,20.2,20.2,20.2,0,0,0-20.2-20.2Zm159.609,0A20.2,20.2,0,1,0,200,67.552,20.2,20.2,0,0,0,179.809,47.35ZM100,52.586a62.944,62.944,0,1,0,62.924,64.526A63.742,63.742,0,0,0,100,52.586Z" transform="translate(-0.001)"/>
        </svg>
        //<svg
        //    className={className}
        //    width={width}
        //    height={height}
        //    viewBox="0 0 64 64"
        //    xmlns="http://www.w3.org/2000/svg"
        //    fill="currentColor"
        //>
        //    {/* 肉球の指部分（4つの丸） */}
        //    <circle cx="20" cy="20" r="4" />
        //    <circle cx="44" cy="20" r="4" />
        //    <circle cx="26" cy="12" r="3.5" />
        //    <circle cx="38" cy="12" r="3.5" />

        //    {/* かかと部分（楕円） */}
        //    <ellipse cx="32" cy="38" rx="10" ry="8" />
        //</svg>
    );
}