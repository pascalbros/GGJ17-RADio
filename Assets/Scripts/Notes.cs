using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Note {
	public string text;
}

public class Notes : MonoBehaviour {

	public static Notes Self;

	[Multiline]
	public string initialNote, initialVoice;

	public UnityEngine.UI.Text text;
	public UnityEngine.UI.Text pageNumber;

	public GameObject prevButton, nextButton;

	private List<Note> notes = new List<Note>();
	private int currentNoteIndex = 0;

	void Awake()
	{
		Self = this;
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(InitC());
	}

	IEnumerator InitC()
	{
		yield return new WaitForSeconds(1);
		AddNote(initialNote, initialVoice);
	}

	public void AddNote(string _note, string voice)
	{
		List<string> tmp = new List<string>(_note.Split(new string[] { "[end]" }, System.StringSplitOptions.RemoveEmptyEntries));
		foreach(string s in tmp)
			_addNote(s);
		lastPagesCount = tmp.Count;
		GoToLastPage();
		playSound(voice);
	}
	void _addNote(string _note)
	{
		Note note = new Note();
		note.text = _note;
		this.AddNote(note);
		this.UpdateNote ();

		ShowNotes();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNoteIndex == 0)
			prevButton.SetActive(false);
		else
			prevButton.SetActive(true);

		if(currentNoteIndex == notes.Count - 1)
			nextButton.SetActive(false);
		else
			nextButton.SetActive(true);
	}

	void AddNote(Note note) {
		this.notes.Add (note);
	}

	void UpdateNote() {
		this.pageNumber.text = "Page " + (this.currentNoteIndex + 1).ToString() + "/" + this.notes.Count.ToString();
		this.text.text = this.notes[this.currentNoteIndex].text;
	}

	public void NextPage() {
		if (this.currentNoteIndex >= this.notes.Count - 1) {
			return;
		}

		this.currentNoteIndex++;
		this.UpdateNote ();
	}

	public void PreviousPage() {
		if (this.currentNoteIndex == 0) {
			return;
		}

		this.currentNoteIndex--;
		this.UpdateNote ();
	}

	int lastPagesCount=1;
	void GoToLastPage() {
		this.currentNoteIndex = this.notes.Count - lastPagesCount;
		this.UpdateNote ();
	}

	public void ShowNotes() {
		Time.timeScale = 0;

		transform.Find("Notes").gameObject.SetActive(true);
		this.GoToLastPage ();
	}

	public void HideNotes() {
		Time.timeScale = 1;

		Player.Self.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		transform.Find("Notes").gameObject.SetActive(false);
		SoundsManager.Self.StopVoiceOff();
	}

	public void playSound(string name) {
		AudioClip audio = SoundsManager.Self.GetClip ("dub/"+name);
		SoundsManager.Self.PlayVoiceOff (audio);
	}
}
