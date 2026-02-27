import { useState, useEffect } from "react";
import usePostsMutations from "../hooks/usePostsMutations";
import usePost from "../hooks/usePost";
import { useParams, useNavigate, Link } from "react-router-dom";
import {
  Box,
  Typography,
  Stack,
  Button,
  Alert,
  CircularProgress,
  TextField,
} from "@mui/material";

const PostEditPage = () => {
  const { id } = useParams();
  const { updatePost, isLoading, error } = usePostsMutations();
  const { post } = usePost(id);
  const [editedPost, setEditedPost] = useState({ title: "", content: "" });
  const navigate = useNavigate();

  useEffect(() => {
    if (!post) return;
    setEditedPost({
      title: post.title ?? "",
      content: post.content ?? "",
    });
  }, [post]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await updatePost(id, editedPost);
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
        Edytuj Post
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
          value={editedPost.title}
          onChange={(e) =>
            setEditedPost({ ...editedPost, title: e.target.value })
          }
          fullWidth
          required
        />
        <TextField
          label="Treść"
          value={editedPost.content}
          onChange={(e) =>
            setEditedPost({ ...editedPost, content: e.target.value })
          }
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

export default PostEditPage;
