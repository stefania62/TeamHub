import React, { useEffect, useState } from "react";
import { getUserTasks, createTask, completeTask } from "../api";

const Tasks = () => {
    const [tasks, setTasks] = useState([]);
    const [newTask, setNewTask] = useState("");
    const [selectedProject, setSelectedProject] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        loadTasks();
    }, []);

    const loadTasks = async () => {
        try {
            const userTasks = await getUserTasks();
            setTasks(userTasks);
        } catch (error) {
            console.error(error);
        }
    };

    const handleCreateTask = async () => {
        if (!newTask || !selectedProject) {
            setError("Please enter task title and select a project.");
            return;
        }

        try {
            await createTask(newTask, selectedProject, null); 
            setNewTask("");
            loadTasks();
        } catch (error) {
            setError(error.message);
        }
    };

    const handleCompleteTask = async (taskId) => {
        await completeTask(taskId);
        loadTasks();
    };

    return (
        <div className="container mt-5">
            <h2>My Tasks</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <div className="mb-3">
                <input
                    type="text"
                    className="form-control"
                    placeholder="Enter Task Title"
                    value={newTask}
                    onChange={(e) => setNewTask(e.target.value)}
                />
                <button className="btn btn-primary mt-2" onClick={handleCreateTask}>
                    Create Task
                </button>
            </div>

            <ul className="list-group">
                {tasks.map((task) => (
                    <li key={task.id} className="list-group-item d-flex justify-content-between">
                        <span>{task.title} {task.isCompleted ? "(Completed)" : ""}</span>
                        {!task.isCompleted && (
                            <button className="btn btn-success btn-sm" onClick={() => handleCompleteTask(task.id)}>
                                Complete
                            </button>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Tasks;
