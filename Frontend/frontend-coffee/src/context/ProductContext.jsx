import React, { createContext, useContext, useEffect, useState } from 'react'
import {
	createProduct,
	deleteProduct,
	getAllProducts,
	getProductById,
	searchAndSortProducts,
	updateProduct,
	updateProductImage,
	updateProductMusic,
} from '../api/services/productService'
import { UserContext } from './UserContext'

export const ProductContext = createContext()

export const ProductProvider = ({ children }) => {
	const [products, setProducts] = useState([])
	const { token, setToken } = useContext(UserContext)

	useEffect(() => {
		const storedToken = localStorage.getItem('token')
		if (storedToken) {
			setToken(storedToken)
		}
	}, [])

	const fetchAllProducts = async () => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await getAllProducts(token)
			setProducts(res.data)
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const getProduct = async id => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			return await getProductById(id, token)
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const createNewProduct = async (productDto, imageFile, musicFile) => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await createProduct(productDto, imageFile, musicFile, token)
			setProducts(prev => [...prev, res.data])
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const updateExistingProduct = async (id, updatedData) => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await updateProduct(id, updatedData, token)
			setProducts(prev => prev.map(p => (p.id === id ? res.data : p)))
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const updateProductImageFile = async (id, imageFile) => {
		if (!token) return Promise.reject(new Error('Token is required'))
		return updateProductImage(id, imageFile, token)
	}

	const updateProductMusicFile = async (id, musicFile) => {
		if (!token) return Promise.reject(new Error('Token is required'))
		return updateProductMusic(id, musicFile, token)
	}

	const removeProduct = async id => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			await deleteProduct(id, token)
			setProducts(prev => prev.filter(p => p.id !== id))
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	const searchProducts = async filter => {
		if (!token) return Promise.reject(new Error('Token is required'))
		try {
			const res = await searchAndSortProducts(filter, token)
			setProducts(res.data)
			return res
		} catch (err) {
			console.error(err.response?.data || err.message)
			throw err
		}
	}

	return (
		<ProductContext.Provider
			value={{
				products,
				fetchAllProducts,
				getProduct,
				createNewProduct,
				updateExistingProduct,
				updateProductImageFile,
				updateProductMusicFile,
				removeProduct,
				searchProducts,
			}}
		>
			{children}
		</ProductContext.Provider>
	)
}
