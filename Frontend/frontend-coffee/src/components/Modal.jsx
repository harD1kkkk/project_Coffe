import React from 'react'
import '../styles/Modal.css'

const Modal = ({ title, isOpen, onClose, onSubmit, children }) => {
	if (!isOpen) return null

	return (
		<div className='modal'>
			<div className='modal-content'>
				<button className='modal-close' onClick={onClose}>
					&times;
				</button>
				<h2>{title}</h2>
				{children}
				<button className='user-page__button' onClick={onSubmit}>
					{title}
				</button>
			</div>
		</div>
	)
}

export default Modal
