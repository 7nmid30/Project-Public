import React from "react";
import { createPortal } from "react-dom";
import "./ModalPortal.css";

const ModalPortal = ({ children, onCancel }) => { //childrenは指定すると子要素を自動で渡してくれる
    return createPortal(
        <div className="modal-overlay" onClick={onCancel}>
            <div
                className="modal-content"
                onClick={e => e.stopPropagation()}
            >
                {children}
            </div>
        </div>,
        document.getElementById("modal-root")
    );
}

export default ModalPortal;
