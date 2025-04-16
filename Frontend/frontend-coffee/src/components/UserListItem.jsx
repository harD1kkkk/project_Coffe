import React from 'react'
import { Link } from 'react-router-dom'

const UserListItem = ({ user, onDelete }) => {
	return (
		<div className='user-list-item'>
			<p>Name: {user.name}</p>
			<p>Email: {user.email}</p>
			<p>Role: {user.role}</p>
			<Link to={`/users/${user.id}`}>
				<button className='blue-btn'>Open</button>
			</Link>
			<button onClick={() => onDelete(user.id)} className='red-btn'>
				Delete
			</button>
		</div>
	)
}

export default UserListItem
