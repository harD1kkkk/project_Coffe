import { productsApi } from '../api'

const getAuthHeaders = token => ({
	headers: {
		Authorization: `Bearer ${token}`,
	},
})

export const getAllProducts = async token => {
	return productsApi.get('/', getAuthHeaders(token))
}

export const getProductById = async (id, token) => {
	return productsApi.get(`/${id}`, getAuthHeaders(token))
}

export const createProduct = async (
	productDto,
	imageFile,
	musicFile,
	token
) => {
	const formData = new FormData()
	formData.append('imageFile', imageFile)
	formData.append('musicFile', musicFile)

	for (const key in productDto) {
		formData.append(key, productDto[key])
	}

	return productsApi.post('/', formData, getAuthHeaders(token))
}

export const updateProduct = async (id, productDto, token) => {
	return productsApi.put(
		`/update-product/${id}`,
		productDto,
		getAuthHeaders(token)
	)
}

export const updateProductImage = async (id, imageFile, token) => {
	const formData = new FormData()
	formData.append('imageFile', imageFile)
	return productsApi.put(
		`/update-imageFile/${id}`,
		formData,
		getAuthHeaders(token)
	)
}

export const updateProductMusic = async (id, musicFile, token) => {
	const formData = new FormData()
	formData.append('musicFile', musicFile)
	return productsApi.put(
		`/update-musicFile/${id}`,
		formData,
		getAuthHeaders(token)
	)
}

export const deleteProduct = async (id, token) => {
	return productsApi.delete(`/${id}`, getAuthHeaders(token))
}

export const searchAndSortProducts = async (filter, token) => {
	return productsApi.put('/search-and-sort', filter, getAuthHeaders(token))
}
