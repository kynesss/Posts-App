import {
  Box,
  Stack,
  Button,
  TextField,
  Typography,
  CircularProgress,
  Alert,
} from "@mui/material";
import { useState } from "react";
import useCreatePost from "../hooks/useCreatePost";
import { useNavigate } from "react-router-dom";

const CreatePostPage = () => {
  const [post, setPost] = useState({ title: "", content: "" });
  const { createPost, isLoading, error } = useCreatePost();
  const navigate = useNavigate();

  const handleCreate = async () => {
    try {
      await createPost(post);
      navigate("/posts");
    } catch {
      //
    }
  };

  return (
    <Box sx={{ maxWidth: 900, m: "auto" }}>
      <Typography variant="h3" sx={{ mb: 3 }}>
        Nowy Post
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Stack spacing={4}>
        <TextField
          label="Tytuł"
          size="medium"
          value={post.title}
          onChange={(e) => setPost({ ...post, title: e.target.value })}
          required
        />
        <TextField
          label="Treść"
          value={post.content}
          onChange={(e) => setPost({ ...post, content: e.target.value })}
          multiline
          minRows={5}
          required
        />
        <Button
          variant="contained"
          size="large"
          sx={{ width: 150 }}
          onClick={handleCreate}
          disabled={isLoading}
        >
          {isLoading ? (
            <CircularProgress size={22} color="inherit" />
          ) : (
            "Wyślij"
          )}
        </Button>
      </Stack>
    </Box>
  );
};

export default CreatePostPage;
