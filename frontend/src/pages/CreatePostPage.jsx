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
import usePostsMutations from "../hooks/usePostsMutations";
import { useNavigate, Link } from "react-router-dom";

const CreatePostPage = () => {
  const [post, setPost] = useState({ title: "", content: "" });
  const { createPost, isLoading, error } = usePostsMutations();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await createPost(post);
      navigate("/posts");
    } catch {
      //
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{ maxWidth: 900, m: "auto" }}
    >
      <Button
        variant="contained"
        size="large"
        component={Link}
        to="/posts"
        sx={{ width: 150, mb: 3 }}
      >
        Wróć
      </Button>

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
          fullWidth
          required
        />
        <TextField
          label="Treść"
          value={post.content}
          onChange={(e) => setPost({ ...post, content: e.target.value })}
          multiline
          minRows={5}
          fullWidth
          required
        />
        <Button
          type="submit"
          variant="contained"
          size="large"
          sx={{ width: 150 }}
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
