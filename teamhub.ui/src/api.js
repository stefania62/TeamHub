import axios from "axios";

export const API_BASE_URL = "https://localhost:7073/api";

const getAuthHeader = () => {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
};

export const getEmployees = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/employees`, getAuthHeader());
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const createProject = async (name) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/projects`,
            { name },
            getAuthHeader()
        );
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const deleteProject = async (projectId) => {
    try {
        await axios.delete(`${API_BASE_URL}/projects/${projectId}`, getAuthHeader());
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

export const getProjects = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/projects`, getAuthHeader());
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const assignEmployeeToProject = async (employeeId, projectId) => {
    try {
        await axios.post(`${API_BASE_URL}/projects/${projectId}/assign`, { employeeId }, getAuthHeader());
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

export const assignTask = async (taskId, employeeId) => {
    try {
        await axios.put(`${API_BASE_URL}/tasks/${taskId}/assign`, { employeeId }, getAuthHeader());
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

export const getUserTasks = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/tasks`, getAuthHeader());
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const createTask = async (title, projectId, assignedUserId) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/tasks`,
            { title, projectId, assignedUserId },
            getAuthHeader()
        );
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const completeTask = async (taskId) => {
    try {
        await axios.put(`${API_BASE_URL}/tasks/${taskId}/complete`, {}, getAuthHeader());
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

