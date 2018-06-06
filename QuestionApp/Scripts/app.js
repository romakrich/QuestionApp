function QuestionsViewModel() {
	var self = this;

	self.questions = ko.observableArray();
	self.question = ko.observable();
	self.distractors = ko.observableArray();
	
	self.filter = ko.observable('');
	self.errorMessage = ko.observable();
	self.questionIsVisible = ko.observable(false);

	self.gridViewModel = new ko.simpleGrid.viewModel({

		data: ko.computed(function () {

			var filter = self.filter().toLowerCase();
			if (!filter) {
				return self.questions();
			} else {
				return ko.utils.arrayFilter(self.questions(), function (item) {
					return item.QuestionContent.toLowerCase().includes(filter);
				});
			}
		}, self),

		columns: [
			{ headerText: "Question", rowText: "QuestionContent" },
			{ headerText: "Answer", rowText: "Answer" }
		],

		pageSize: 10
	});

	self.getQuestion = function (item) {

		self.question(item);
		self.distractors(ko.utils.arrayMap(item.Distractors, function (distractor) {
			return { value: ko.observable(distractor) };
		}));

		self.questionIsVisible(true);
	};

	self.addQuestion = function () {

		var q = {
			Id: -1,
			QuestionContent: '',
			Answer: 0,
			Distractors: ko.observableArray([{ value: ko.observable() }, { value: ko.observable() }, { value: ko.observable() }, { value: ko.observable() }])
		};

		self.question(q);
		self.distractors(q.Distractors());
		self.questionIsVisible(true);
	};

	self.saveQuestion = function () {

		self.errorMessage('');

		self.question().Distractors = ko.utils.arrayMap(self.distractors(), function (distractor) {
			if (distractor.value() === null || distractor.value() === '') {
				self.errorMessage('Distractor(s) can not be blank');
			}
			else if (distractor.value() === self.question().Answer) {
				self.errorMessage('Answer can not be one of the distractors');
			}
			return distractor.value();
		});

		if (self.question().Distractors.length === 0) {
			self.errorMessage('Question requires at least one distractor');
		}
		else if (self.question().QuestionContent === '') {
			self.errorMessage('Question is required');
		}
		else if (self.question().Answer === '') {
			self.errorMessage('Answer is required');
		}
		else if (self.question().Distractors.length !== new Set(self.question().Distractors).size) {
			self.errorMessage('All distractors must be unique');
		}

		if (self.errorMessage() !== '') {
			return;
		}

		// new question
		if (self.question().Id === -1) {
			ajaxHelper(questionUri, 'POST', self.question).done(function () {

				self.question().Id = self.questions().length;
				self.questions.push(self.question());
			});
		}
		else {
			ajaxHelper(questionUri + self.question().Id, 'PUT', self.question).done(function () {
				var q = {
					Id: self.question().Id,
					QuestionContent: self.question().QuestionContent,
					Answer: self.question().Answer,
					Distractors: self.question().Distractors
				};

				self.questions.replace(self.question(), q);
			});
		}

		self.questionIsVisible(false);
		self.distractors(null);
	};

	self.deleteQuestion = function (item) {

		ajaxHelper(questionUri + item.Id, 'DELETE').done(function () {
			self.questions.remove(item);
		});
	};

	self.deleteDistractor = function (item) {
		self.distractors.remove(item);
	};

	self.addDistractor = function () {
		self.distractors.push({ value: ko.observable() });
	};

	this.sortByQuestion = function () {
		this.questions.sort(function (a, b) {
			return a.QuestionContent < b.QuestionContent ? -1 : 1;
		});
	};


	var questionUri = '/api/Question/';

	function ajaxHelper(uri, method, data) {
		self.errorMessage('');

		return $.ajax({
			type: method,
			url: uri,
			dataType: 'json',
			contentType: 'application/json',
			data: data ? ko.toJSON(data) : null
		}).fail(function (jqXHR, textStatus, errorThrown) {
			self.errorMessage(errorThrown);
		});
	}

	function getAllQuestions() {
		ajaxHelper(questionUri, 'GET').done(function (data) {
			self.questions(data);
		});

	}

	getAllQuestions();
}

ko.applyBindings(new QuestionsViewModel());










